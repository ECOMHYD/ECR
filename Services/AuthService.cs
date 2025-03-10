using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

   public async Task<string> Register(RegisterModel registerModel)
{
    if (await _userRepository.UserExists(registerModel.Username))
    {
        throw new Exception("Username already exists.");
    }

    var encryptedPassword = EncryptionHelper.Encrypt(registerModel.Password);

    var user = new User
    {
        Username = registerModel.Username,
        Email = registerModel.Email,
        PhoneNumber = registerModel.PhoneNumber,
        EncryptedPassword = encryptedPassword, // Store encrypted password
        Role = "User"
    };

    await _userRepository.AddUser(user);
    return "User registered successfully";
}



   public async Task<string> Login(LoginModel model)
{
    var user = await _userRepository.GetByUsernameOrEmail(model.UsernameOrEmail);
    
    if (user == null)
    {
        Console.WriteLine("❌ User not found.");
        return "Invalid credentials";
    }

    string decryptedPassword = EncryptionHelper.Decrypt(user.EncryptedPassword);
    
    if (decryptedPassword != model.Password)
        return "Invalid credentials";

    return GenerateJwtToken(user);
}



    private string GenerateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]); // Change _configuration to _config

    var claims = new List<Claim>
    {
         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role) // Include user role in token
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(3),
         Issuer = _config["Jwt:Issuer"],
        Audience = _config["Jwt:Audience"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}


}
