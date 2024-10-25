namespace BackendPetMinimalAPI.date;

public interface IUserRepository : IDisposable
{
    Task InsertUserAsync(User user);
    Task<List<User>> GetUsersAsync();
    Task<List<User>> GetUsersAsync(string userName);
    Task<User> GetUserAsync(int userId);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int userId);
    Task SaveAsync();
}
