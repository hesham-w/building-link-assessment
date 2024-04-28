using Api.Domain;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetAll();

    Task<Driver> GetById(int driverId);

    Task<Driver> Insert(Driver driver);

    Task<Driver> Update(Driver driver);

    Task Delete(int driverId);
}

public class DriverRepository : IDriverRepository
{
    private readonly string _connectionString;

    public DriverRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Default connection string must be specified.");
    }

    public async Task<IEnumerable<Driver>> GetAll()
    {
        using var _connection = new SqliteConnection(_connectionString);

        var sql = @"SELECT Id, Name, AddressLine1, AddressLine2, PhoneNumber 
                    FROM Driver";

        return await _connection.QueryAsync<Driver>(sql);
    }

    public async Task<Driver> GetById(int driverId)
    {
        using var _connection = new SqliteConnection(_connectionString);

        var sql = @"SELECT Id, Name, AddressLine1, AddressLine2, PhoneNumber 
                    FROM Driver 
                    WHERE Id = @driverId";

        return await _connection.QuerySingleOrDefaultAsync<Driver>(sql, new { driverId });
    }

    public async Task<Driver> Insert(Driver driver)
    {
        driver.CreatedOn = DateTime.Now;

        using var _connection = new SqliteConnection(_connectionString);

        var sql = $@"INSERT INTO Driver
                               (Name,
                                AddressLine1,
                                AddressLine2,
                                PhoneNumber)
                         VALUES
                               (@Name,
                                @AddressLine1,
                                @AddressLine2,
                                @PhoneNumber);
                    SELECT last_insert_rowid();";

        var driverId = await _connection.QuerySingleAsync<int>(sql, driver);

        driver.Id = driverId;

        return driver;
    }

    public async Task<Driver> Update(Driver driver)
    {
        driver.ModifiedOn = DateTime.Now;

        using var _connection = new SqliteConnection(_connectionString);

        var sql = @"UPDATE Driver
                    SET
                        Name = @Name,
                        AddressLine1 = @AddressLine1,
                        AddressLine2 = @AddressLine2,
                        PhoneNumber = @PhoneNumber
                    WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, driver);
        return driver;
    }

    public async Task Delete(int driverId)
    {
        using var _connection = new SqliteConnection(_connectionString);

        var sql = @"DELETE 
                    FROM Driver
                    WHERE Id = @driverId";

        await _connection.ExecuteAsync(sql, new { driverId });
    }
}
