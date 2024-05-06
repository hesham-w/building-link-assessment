using Api.Domain;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);

    Task<bool> ExistsAsync(string email);

    Task<User> InsertAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Default connection string must be specified.");
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        using var _connection = new SqliteConnection(_connectionString);

        var sql = @"SELECT Id, Name, Email, Password 
                    FROM User 
                    WHERE Email = @email";

        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
    }

    public async Task<User> InsertAsync(User user)
    {
        user.CreatedOn = DateTime.Now;

        using var _connection = new SqliteConnection(_connectionString);

        var sql = $@"INSERT INTO User
                               (Name,
                                Email,
                                Password)
                         VALUES
                               (@Name,
                                @Email,
                                @Password);
                    SELECT last_insert_rowid();";

        var userId = await _connection.QuerySingleAsync<int>(sql, user);

        user.Id = userId;

        return user;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        using var _connection = new SqliteConnection(_connectionString);

        var sql = @"SELECT EXISTS(SELECT 1 FROM User WHERE Email = @email)";

        return await _connection.QuerySingleAsync<bool>(sql, new { email });
    }
}
