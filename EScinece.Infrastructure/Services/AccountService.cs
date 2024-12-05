using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Repositories;

namespace EScinece.Infrastructure.Services;

public class AccountService(AccountRepository accountRepository, IUserService userService) : IAccountService
{
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly IUserService _userService = userService;

    public async Task<Result<Account, string>> Create(AccountDto data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            return "Name is required";
        }
        

        var account = Account.Create(data.Name, data.User, data.Role);

        if (!account.onSuccess)
        {
            return account.Error;
        }

        var createResult = await _accountRepository.Create(account.Value);

        return createResult;
    }

    public async Task<Account?> FindByEmail(string email)
    {
        var user = await _userService.FindByEmail(email);

        if (user == null)
            return null;

        return await _accountRepository.GetByUserId(user.Id);
    }

    public Task<Account?> FindById(Guid id)
    {
        throw new NotImplementedException();
    }
}
