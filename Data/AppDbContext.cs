using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }  // ✅ Add Cart table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Fruits" },
            new Category { CategoryId = 2, Name = "Vegetables" },
            new Category { CategoryId = 3, Name = "Ice Creams" },
            new Category { CategoryId = 4, Name = "Groceries" }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Apple", Price = 50.00m, Quantity = 100, CategoryId = 1 },
            new Product { ProductId = 2, Name = "Banana", Price = 20.00m, Quantity = 150, CategoryId = 1 },
            new Product { ProductId = 3, Name = "Carrot", Price = 30.00m, Quantity = 200, CategoryId = 2 },
            new Product { ProductId = 4, Name = "Tomato", Price = 25.00m, Quantity = 250, CategoryId = 2 },
            new Product { ProductId = 5, Name = "Vanilla Ice Cream", Price = 100.00m, Quantity = 50, CategoryId = 3 },
            new Product { ProductId = 6, Name = "Chocolate Ice Cream", Price = 120.00m, Quantity = 40, CategoryId = 3 },
            new Product { ProductId = 7, Name = "Rice", Price = 200.00m, Quantity = 300, CategoryId = 4 },
            new Product { ProductId = 8, Name = "Sugar", Price = 40.00m, Quantity = 400, CategoryId = 4 }
        );

        // Define unique constraint for user-product in cart (each user can have only one entry per product)
        modelBuilder.Entity<Cart>()
            .HasIndex(c => new { c.UserId, c.ProductId })
            .IsUnique();
    }
}
