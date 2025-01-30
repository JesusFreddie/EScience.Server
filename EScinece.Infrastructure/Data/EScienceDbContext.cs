using System.Data;
using Npgsql;

namespace EScinece.Infrastructure.Data;

public class EScienceDbContext(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        try
        {
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(token);

            return connection;
        }
        catch (NpgsqlException ex)
        {
            throw new DbConnectionException($"Failed to connect to the database: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new DbConnectionException($"An unexpected error occurred while creating a database connection: {ex.Message}", ex);
        }
    }
}

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class DbConnectionException : Exception
{
    public DbConnectionException(string message, Exception innerException) : base(message, innerException) {}
}