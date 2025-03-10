using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await _authService.Register(model);
        if (result == "Username already exists") return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await _authService.Login(model);
        if (token == "Invalid credentials") return Unauthorized(token);
        return Ok(new { Token = token });
    }
}
