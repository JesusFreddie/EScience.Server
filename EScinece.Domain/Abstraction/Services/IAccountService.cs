using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IAccountService
{
    public Task<Result<Account, string>> Create(AccountDto data);
    public Task<Account?> FindById(Guid id);
    public Task<Account?> FindByEmail(string email);
}
