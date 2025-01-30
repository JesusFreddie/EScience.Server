using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Services;

public class AccountService(IAccountRepository accountRepository, IUserService userService) : IAccountService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IUserService _userService = userService;

    public async Task<Result<AccountDto, string>> Create(Guid userId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return AccountErrorMessage.NameIsRequired;
        }

        var account = Account.Create(name, userId);

        if (!account.onSuccess)
        {
            return account.Error;
        }

        await _accountRepository.Create(account.Value);

        return new AccountDto(
            Id: account.Value.Id,
            Role: account.Value.Role,
            UserId: userId,
            Name: name
            );
    }

    public async Task<AccountDto?> GetById(Guid id)
    {
        var account = await _accountRepository.GetById(id);
        
        if (account is null)
            return null;
        
        return new AccountDto(
            Id: account.Id,
            Role: account.Role,
            UserId: account.UserId,
            Name: account.Name
            );
    }

    public async Task<AccountDto?> GetByEmail(string email)
    {
        var user = await _userService.GetByEmail(email);
        if (user is null)
            return null;

        var account = await _accountRepository.GetByUserId(user.Id);

        if (account is null)
            return null;

        return new AccountDto(
            Id: account.Id,
            Role: account.Role,
            Name: account.Name,
            UserId: user.Id
        );
    }
}
