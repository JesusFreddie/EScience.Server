using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction;

public interface IAccountService
{
    public Task<Result<AccountDto, string>> Create(AccountDto data);
}
