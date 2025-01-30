using System;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IAccountRepository
{
    Task Create(Account account);
    Task<Guid> Update(Guid id, string? name);
    Task<Guid> Delete(Guid id);
    Task<Account?> GetById(Guid id);
    Task<Account?> GetByUserId(Guid id);
}