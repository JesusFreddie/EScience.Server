using System;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;

namespace EScinece.Infrastructure.Repositories;

public class AccountRepository(EScienceDbContext _context) : IAccountRepository
{
    public async Task<Account> Create(Account account)
    {
        throw new NotImplementedException();
    }

    public Task<Account> Update(Account account)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetByUserId(Guid id)
    {
        throw new NotImplementedException();
    }
}
