using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required]
    public string UsernameOrEmail { get; set; }

    [Required]
    public string Password { get; set; }
}
