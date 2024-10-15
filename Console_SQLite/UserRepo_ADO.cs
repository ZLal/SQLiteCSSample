using Microsoft.Data.Sqlite;

namespace Console_SQLite
{
    public class UserRepo_ADO : IUserRepo
    {
        SqliteConnection _connection;
        public UserRepo_ADO(string filePath)
        {
            bool isNewDB = !File.Exists(filePath);
            _connection = new SqliteConnection($"Data Source={filePath};");
            _connection.Open();
            if (isNewDB)
            {
                using var command = _connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL)";
                _ = command.ExecuteNonQuery();
            }
        }
        ~UserRepo_ADO()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public async Task CreateAsync(List<User> users)
        {
            foreach (var user in users)
            {
                using var command = _connection.CreateCommand();
                command.CommandText = "Insert Into Users (Name) Values ($name)";
                command.Parameters.AddWithValue("$name", user.Name);
                _ = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "Select * From Users";
            using var reader = await command.ExecuteReaderAsync();
            List<User> result = new();
            while (reader.Read())
            {
                result.Add(new User() { Id = reader.GetInt32(0), Name = reader.GetString(1) });
            }
            return result;
        }
    }
}
