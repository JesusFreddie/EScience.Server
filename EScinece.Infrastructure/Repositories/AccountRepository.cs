using System;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;

namespace EScinece.Infrastructure.Repositories;

public class AccountRepository(EScienceDbContext _context) : IAccountRepository
{
    public async Task<Account> Create(Account account)
    {
        await _context.Account.AddAsync(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public Task<Account> Update(Account account)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        var account = await _context.Account.FindAsync(id);
        if (account is null)
        {
            return false;
        }

        _context.Account.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Account?> GetByUserId(Guid id)
    {
        return await _context.Account.FindAsync(id);
    }
}
