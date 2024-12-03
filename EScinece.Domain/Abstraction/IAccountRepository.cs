using System;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction;

public interface IAccountRepository
{
    Task<Account?> Create(Account account);
    Task<Account?> Update(Account account);
    Task<bool> Delete(Guid id);
    Task<Account?> GetByUserId(Guid id);
}