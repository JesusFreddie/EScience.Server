using System;
using System.Text.Json;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace EScinece.Infrastructure.Repositories;

public class AccountRepository(EScienceDbContext context, IDistributedCache cache) : IAccountRepository
{
    private readonly EScienceDbContext _context = context;
    private readonly IDistributedCache _cache = cache;
    public async Task<Account> Create(Account account)
    {
        await _context.Account.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Guid> Update(Guid id, string? name)
    {
        await _context.Account
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(
                a => a
                    .SetProperty(c => c.Name, name)
                );
        await _cache.RemoveAsync("account" + id.ToString());
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Account
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }

    public async Task<Account?> GetByUserId(Guid id)
    {
        return await _context.Account.FindAsync(id);
    }

    public async Task<Account?> GetById(Guid id)
    {
        // var accountCache = await _cache.GetStringAsync("account:" + id.ToString());
        // if (accountCache is not null)
        // {
        //     return JsonSerializer.Deserialize<Account>(accountCache);
        // }

        var account = await _context.Account.FindAsync(id);
        // await _cache.SetStringAsync("account:" + id.ToString(), JsonSerializer.Serialize(account), new DistributedCacheEntryOptions
        // {
        //     AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        // });
        return account;
    }
}
