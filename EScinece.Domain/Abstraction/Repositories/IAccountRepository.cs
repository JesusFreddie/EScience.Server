using System;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    public Task<Account?> GetByUserId(Guid id);
    public Task<Guid?> Update(Guid id, string? name);
}