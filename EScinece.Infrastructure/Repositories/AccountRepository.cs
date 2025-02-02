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
            return await connection.QueryAsync<Account>("SELECT * FROM accounts");
        });

    public async Task Create(Account account) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO accounts (id, name, role, user_id)
                VALUES (@id, @name, @role, @userId)
                """, account);

            await cache.SetStringAsync("account:" + account.Id, JsonSerializer.Serialize(account),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
            
            return Task.CompletedTask;
        });

    public async Task<Guid?> Update(Guid id, string? name) =>
        await ExecuteWithExceptionHandlingAsync<Guid?>(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();

            var updateSet = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                updateSet.Add("name = @name");
                parameters.Add("name", name);
            }

            if (updateSet.Count == 0)
                return null;

            var sql = $"UPDATE accounts SET {string.Join(", ", updateSet)}  WHERE id = @id";
            var result = await connection.ExecuteAsync(sql, new { id, name });

            return result > 0 ? id : null;
        });

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetByUserId(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Account>(
                "SELECT * FROM accounts WHERE user_id = @userId", new { userId = id });
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
                .QueryFirstOrDefaultAsync<Account>("SELECT * FROM accounts WHERE Id = @id", new { id });

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
