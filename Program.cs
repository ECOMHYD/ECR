using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartService, CartService>();       // ✅ Add Cart Service
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddControllers();
// Configure Authentication & JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// ✅ **Add this line to fix the error**
builder.Services.AddAuthorization();

var app = builder.Build();

// Apply Migrations & Seed Admin
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Users.Any(u => u.Username == "admin"))
    {
        var password = "Admin@123";
        var encryptedPassword = EncryptionHelper.Encrypt(password);

        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            PhoneNumber = "9876543210",
            EncryptedPassword = encryptedPassword,  // Ensure property matches the model
            Role = "Admin"
        };

        dbContext.Users.Add(adminUser);
        dbContext.SaveChanges();
    }
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();  // ✅ **Now Authorization services exist**
app.MapControllers();
app.Run();
