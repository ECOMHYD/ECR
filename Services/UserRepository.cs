using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByUsernameOrEmail(string usernameOrEmail) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

    public async Task<bool> UserExists(string username) =>
        await _context.Users.AnyAsync(u => u.Username == username);

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
