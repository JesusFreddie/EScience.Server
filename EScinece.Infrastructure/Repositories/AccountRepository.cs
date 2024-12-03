using System;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;

namespace EScinece.Infrastructure.Repositories;

public class AccountRepository(EScienceDbContext context) : IAccountRepository
{
    public async Task<Account?> Create(Account account)
    {
        await context.Account.AddAsync(account);
        await context.SaveChangesAsync();
        return account;
    }

    public Task<Account?> Update(Account account)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        var account = await context.Account.FindAsync(id);
        if (account is null)
        {
            return false;
        }

        context.Account.Remove(account);
        await context.SaveChangesAsync();
        return true;
    }

    public Task<Account?> GetByUserId(Guid id)
    {
        throw new NotImplementedException();
    }
}
