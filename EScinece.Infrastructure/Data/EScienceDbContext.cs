using System.Data;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace EScinece.Infrastructure.Data;

public class EScienceDbContext : IDbConnectionFactory, IDisposable, IAsyncDisposable
{
    private readonly string _connectionString;
    private readonly ILogger _logger;
    private NpgsqlConnection? _connection;
    
    public EScienceDbContext(ILogger logger, string connectionString)
    {
        _logger = logger;
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        if (_connection is not null && _connection.State != ConnectionState.Open)
            return _connection;
        
        try
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(token);

            return connection;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError($"Не удалось подключиться к базе данных {ex.Message}", ex);
            throw new DbConnectionException($"Не удалось подключиться к базе данных {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError($"При создании подключения к базе данных произошла непредвиденная ошибка: {ex.Message}", ex);
            throw new DbConnectionException($"При создании подключения к базе данных произошла непредвиденная ошибка: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null) await _connection.DisposeAsync();
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