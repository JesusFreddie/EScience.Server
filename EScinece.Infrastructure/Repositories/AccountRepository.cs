using System.Text.Json;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace EScinece.Infrastructure.Repositories;

public class AccountRepository(IDbConnectionFactory connectionFactory, IDistributedCache cache) : IAccountRepository
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
        
        using var connection = await connectionFactory.CreateConnectionAsync();
        var account = await connection.QueryFirstOrDefaultAsync<Account>("SELECT * FROM accounts WHERE Id = @id", new { id });
        if (account is null)
            return null;
        
        await cache.SetStringAsync("account:" + id, JsonSerializer.Serialize<Account>(account), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        });

        return account;
    }
}
