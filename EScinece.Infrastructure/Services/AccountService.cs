using System;
using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Repositories;

namespace EScinece.Infrastructure.Services;

public class AccountService(AccountRepository accountRepository) : IAccountService
{
    private readonly AccountRepository _accountRepository = accountRepository;

    public async Task<Result<AccountDto, string>> Create(AccountDto data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            return "Name is required";
        }
        

        var account = Account.Create(data.Name, data.User, data.Role);

        if (!account.IsOk)
        {
            return account.Error;
        }

        var createResult = await _accountRepository.Create(account.Value);

        return new AccountDto(
            Name: createResult.Name,
            User: createResult.User,
            Role: createResult.Role
        );
    }
}
