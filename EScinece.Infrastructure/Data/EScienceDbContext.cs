using System.Data;
using Dapper;
using Npgsql;

namespace EScinece.Infrastructure.Data;

public class EScienceDbContext(string connectionString) : IEScienceDbContext
{
    private readonly string _connectionString = connectionString;

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(token);
        return connection;
    }

    private async Task _initTables()
    {
        await _initUsers();
        
        async Task _initUsers()
        {
            var sql = $"""
                CREATE TABLE IF NOT EXISTS Accounts (
                    Id SERIAL PRIMARY KEY, 
                    Name VARCHAR(255),
                    Role INTEGER, 
                    UserId UUID NOT NULL,
                    CONSTRAINT fk_accounts_users FOREIGN KEY (UserId) REFERENCES Users(Id)
                );
                CREATE INDEX IF NOT EXISTS accounts_users ON Accounts (UserId);
                """;
            await (await CreateConnectionAsync()).ExecuteAsync(sql);
        }
    }
}

public interface IEScienceDbContext
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}