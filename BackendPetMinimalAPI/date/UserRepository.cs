using Microsoft.EntityFrameworkCore;

namespace BackendPetMinimalAPI.date;

public class UserRepository : IUserRepository
{
    private readonly UserDb _context;

    public UserRepository(UserDb context)
    {
        _context = context;
    }

    public async Task InsertUserAsync(User user) => await _context.Users.AddAsync(user);

    public Task<List<User>> GetUsersAsync() => _context.Users.ToListAsync();

    public Task<List<User>> GetUsersAsync(string userName) =>
        _context.Users.Where(u => u.UserName.Contains(userName)).ToListAsync();
    
    public async Task<User> GetUserAsync(int userId) =>
        await _context.Users.FindAsync(new object[] { userId });

    public async Task UpdateUserAsync(User user)
    {
        var userFromDb = await _context.Users.FindAsync(new object[] {user.Id});
        if (userFromDb == null) return;
        userFromDb.Age = user.Age;
        userFromDb.UserName = user.UserName;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var userFromDb = await _context.Users.FindAsync(new object[] { userId });
        if (userFromDb == null) return;
        _context.Users.Remove(userFromDb);
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}