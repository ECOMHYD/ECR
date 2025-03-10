using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User> GetByUsernameOrEmail(string usernameOrEmail);
    Task<bool> UserExists(string username);
    Task AddUser(User user);
}
