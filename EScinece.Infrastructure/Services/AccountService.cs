using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class AccountService(
    IAccountRepository accountRepository, 
    IUserService userService,
    ILogger<AccountService> logger
    ) : IAccountService
{
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

        try
        {
            await accountRepository.Create(account.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка создания аккаунта");
            throw new Exception("Произошла серверная ошибка создания аккаунта");
        }

        return new AccountDto(
            Id: account.Value.Id,
            Role: account.Value.Role,
            UserId: userId,
            Name: name
            );
    }

    public async Task<AccountDto?> GetById(Guid id)
    {
        Account? account;
        try
        {
            account = await accountRepository.GetById(id);
        }
        catch
        {
            throw new Exception(AccountErrorMessage.ServerErrorGetAccount);
        }
        
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
        try
        {
            var user = await userService.GetByEmail(email);
            if (user is null)
                return null;

            var account = await accountRepository.GetByUserId(user.Id);

            if (account is null)
                return null;

            return new AccountDto(
                Id: account.Id,
                Role: account.Role,
                Name: account.Name,
                UserId: user.Id
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка получения аккаунта");
            throw new Exception("Произошла серверная ошибка получения аккаунта");
        }
    }

    public async Task<AccountDto?> GetByUserId(Guid id)
    {
        try
        {
            var account = await accountRepository.GetByUserId(id);
            if (account is null)
                return null;
            
            return new AccountDto(
                Id: account.Id,
                UserId: account.UserId,
                Role: account.Role,
                Name: account.Name);
        }
        catch
        {
            throw new Exception(AccountErrorMessage.ServerErrorGetAccount);
        }
    }

    public async Task<ProfileDto?> GetProfile(Guid accountId)
    {
        try
        {
            var account = await accountRepository.GetById(accountId);
            if (account is null)
                return null;
            
            var user = await userService.GetById(account.UserId);

            if (user is null)
                return null;

            return new ProfileDto(
                Id: account.Id,
                Email: user.Email,
                Name: account.Name
                );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Server error getting profile");
        }
    }

    public async Task<ProfileDto?> GetProfile(string name)
    {
        try
        {
            var account = await accountRepository.GetByName(name);
            if (account is null)
                return null;
            
            var user = await userService.GetById(account.UserId);

            if (user is null)
                return null;

            return new ProfileDto(
                Id: account.Id,
                Email: user.Email,
                Name: account.Name
                );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Server error getting profile");
        }
    }

    public async Task<AccountDto?> GetByName(string name)
    {
        try
        {
            var account = await accountRepository.GetByName(name);
            if (account is null)
                return null;
            
            return new AccountDto(
                Id: account.Id,
                UserId: account.UserId,
                Role: account.Role,
                Name: account.Name);
        }
        catch
        {
            throw new Exception(AccountErrorMessage.ServerErrorGetAccount);
        }
    }
}
