using Microsoft.Data.Sqlite;
using Dapper;
// PackageReference Include="Dapper" Version="2.1.35"

namespace Console_SQLite
{
    public class UserRepo_Dapper : IUserRepo
    {
        SqliteConnection _connection;
        public UserRepo_Dapper(string filePath)
        {
            bool isNewDB = !File.Exists(filePath);
            _connection = new SqliteConnection($"Data Source={filePath};");
            _connection.Open();
            if (isNewDB)
            {
                _connection.Execute("CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL)");
            }
        }
        ~UserRepo_Dapper()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public async Task CreateAsync(List<User> users)
        {
            var names = users.Select(x => new { name = x.Name }).ToArray();
            _ = await _connection.ExecuteAsync("Insert Into Users (Name) Values (@name)", names);
        }

        public async Task<List<User>> GetAllAsync()
        {
            var result = await _connection.QueryAsync<User>("Select * From Users");
            return result.ToList();
        }
    }
}
