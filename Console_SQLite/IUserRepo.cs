
namespace Console_SQLite;

public interface IUserRepo
{
    public Task CreateAsync(List<User> users);
    public Task<List<User>> GetAllAsync();
}
