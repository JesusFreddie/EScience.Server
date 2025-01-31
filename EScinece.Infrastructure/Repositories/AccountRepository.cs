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

public class AccountRepository(IDbConnectionFactory connectionFactory, IDistributedCache cache, ILogger<AccountRepository> logger) : IAccountRepository
{
    public async Task Create(Account account)
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
    }

    public async Task<Guid> Update(Guid id, string? name)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetByUserId(Guid id)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<Account>(
            "SELECT * FROM accounts WHERE user_id = @userId", new { UserId = id });
    }

    public async Task<Account?> GetById(Guid id)
    {
        var accountCache = await cache.GetStringAsync("account:" + id);
        if (accountCache is not null)
        {
            return JsonSerializer.Deserialize<Account>(accountCache);
        }

        Account? account;

        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            account =
                await connection.QueryFirstOrDefaultAsync<Account>("SELECT * FROM accounts WHERE Id = @id", new { id });
        }
        catch (NpgsqlException ex)
        {
            logger.LogError("Произошла ошибка sql зпроса получения account", ex);
            throw new Exception(ex.Message, ex);
        }
        catch (DbConnectionException ex)
        {
            logger.LogError("Ошибка соединения при получении account", ex);
            throw new Exception(ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Произошла неизвестная ошибка при получении account", ex);
            throw new Exception("Произошла неизвестная ошибка при получении account", ex);
        }
        
        if (account is null)
            return null;

        try
        {
            await cache.SetStringAsync("account:" + id, JsonSerializer.Serialize<Account>(account),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                });
        }
        catch (RedisConnectionException ex)
        {
            logger.LogError("Ошибка соединения с redis");
            throw new Exception(ex.Message, ex);
        }
        catch (RedisException ex)
        {
            logger.LogError("Произошла ошибка redis", ex);
            throw new Exception(ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Произошла неизвестная ошибка при запись account в redis");
            throw new Exception(ex.Message, ex);
        }

        return account;
    }
}
