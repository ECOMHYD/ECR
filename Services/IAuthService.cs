using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> Register(RegisterModel registerModel);
    Task<string> Login(LoginModel model);
}
