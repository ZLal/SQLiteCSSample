using Microsoft.EntityFrameworkCore;
// PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="7.0.20"

namespace Console_SQLite
{
    public class UserRepo_EF : IUserRepo
    {
        SQLiteContext _db;
        public UserRepo_EF(string filePath)
        {
            _db = new SQLiteContext($"Data Source={filePath};");
        }
        ~UserRepo_EF()
        {
            _db.Database.CloseConnection();
            _db.Dispose();
        }

        public async Task CreateAsync(List<User> users)
        {
            await _db.AddRangeAsync(users);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }
    }

    public class SQLiteContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        string conStr;
        public SQLiteContext(string conStr)
        {
            Database.EnsureCreated();
            this.conStr = conStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(conStr);
        }
    }
}
