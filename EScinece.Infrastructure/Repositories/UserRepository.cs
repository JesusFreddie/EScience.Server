using System.Data;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository(
    IDbConnectionFactory connectionFactory, 
    ILogger<UserRepository> logger
    ) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<User>("SELECT * FROM users");
        });

    public async Task Create(User user) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO users (id, email, hashed_password)
                VALUES (@Id, @Email, @HashedPassword)
                """, user);
            return Task.CompletedTask;
        });

    public async Task<User?> GetByEmail(string email) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE email = @email", new { email });
            return user;
        });

    public async Task<bool> Delete(Guid id) => 
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync("DELETE FROM users WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<User?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @id", new { id });
        });

    public Task<Guid?> Update(User entity)
    {
        throw new NotImplementedException();
    }

    private async Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (NpgsqlException ex)
        {
            logger.LogError("Произошла ошибка SQL-запроса", ex);
            throw new Exception("Ошибка SQL-запроса: " + ex.Message, ex);
        }
        catch (RedisConnectionException ex)
        {
            logger.LogError("Ошибка соединения с Redis", ex);
            throw new Exception("Ошибка соединения с Redis: " + ex.Message, ex);
        }
        catch (DbConnectionException ex)
        {
            logger.LogError("Ошибка соединения", ex);
            throw new Exception("Ошибка соединения: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Неизвестная ошибка", ex);
            throw new Exception("Неизвестная ошибка: " + ex.Message, ex);
        }
    }
}