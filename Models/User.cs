using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string EncryptedPassword { get; set; } // Store encrypted password

    [Required]
    public string MaskedPassword => "*******"; // Always return masked password

    [Required]
    public string Role { get; set; } = "User"; // Default role is 'User'

    // **Navigation property**
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
