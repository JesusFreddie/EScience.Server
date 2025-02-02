using System;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByUserId(Guid id);
    public Task<Guid?> Update(Guid id, string? name);
}