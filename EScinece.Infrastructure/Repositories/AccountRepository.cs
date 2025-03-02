using System.Text.Json;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class AccountRepository(
    IDbConnectionFactory connectionFactory, 
    IDistributedCache cache, 
    ILogger<AccountRepository> logger
    ) : IAccountRepository
{
    public async Task<IEnumerable<Account>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Account>("SELECT * FROM accounts WHERE deleted_at IS NULL");
        });

    public async Task Create(Account account) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO accounts (id, name, user_id, role_id)
                VALUES (@id, @name, @userId, @roleId)
                """, account);

            await cache.SetStringAsync("account:" + account.Id, JsonSerializer.Serialize(account),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
            
            return Task.CompletedTask;
        });

    public async Task Update(Account entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                UPDATE accounts 
                SET name = @name, 
                    updated_at = NOW()
                WHERE id = @id AND deleted_at IS NULL
                """, entity);

            return Task.CompletedTask;
        });

    public async Task<bool> Delete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync($"DELETE FROM accounts WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<bool> SoftDelete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                "UPDATE accounts SET deleted_at = NOW() WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<Account?> GetByUserId(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Account>(
                "SELECT * FROM accounts WHERE user_id = @userId AND deleted_at IS NULL", new { userId = id });
        });

    public async Task<Account?> GetByName(string name) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Account>(
                "SELECT * FROM accounts WHERE name = @name AND deleted_at IS NULL", new { name });
        });


    public async Task<Account?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var accountCache = await cache.GetStringAsync("account:" + id);
            if (accountCache is not null)
            {
                return JsonSerializer.Deserialize<Account>(accountCache);
            }

            using var connection = await connectionFactory.CreateConnectionAsync();
            var account = await connection
                .QueryFirstOrDefaultAsync<Account>(
                    "SELECT * FROM accounts WHERE Id = @id AND deleted_at IS NULL", new { id });

            if (account is null)
                return null;

            await cache.SetStringAsync("account:" + id, JsonSerializer.Serialize<Account>(account),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                });

            return account;
        });

    private async Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "Произошла ошибка SQL-запроса");
            throw new Exception("Ошибка SQL-запроса: " + ex.Message, ex);
        }
        catch (RedisConnectionException ex)
        {
            logger.LogError(ex, "Ошибка соединения с Redis");
            throw new Exception("Ошибка соединения с Redis: " + ex.Message, ex);
        }
        catch (DbConnectionException ex)
        {
            logger.LogError(ex, "Ошибка соединения");
            throw new Exception("Ошибка соединения: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Неизвестная ошибка");
            throw new Exception("Неизвестная ошибка: " + ex.Message, ex);
        }
    }
}
