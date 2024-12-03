using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Services;

public class AuthService(IUserService userService, IAccountService accountService) : IAuthService
{
    private readonly IUserService _userService = userService;
    private readonly IAccountService _accountService = accountService;

    public Task<Result<UserDto, string>> Login(UserDto user)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<AccountDto, string>> Register(RegisterDto data)
    {
        var createResult = await _userService.Create(new UserDto(
            Email: data.Email,
            Password: data.Password,
            Id: Guid.Empty
        ));

        if (!createResult.IsOk)
            return createResult.Error;

        var accountResult = await _accountService.Create(new AccountDto(
            User: createResult.Value,
            Name: data.Name,
            Id: Guid.Empty
        ));

        if (!accountResult.IsOk)
        {
            await _userService.Delete(createResult.Value.Id);
            return accountResult.Error;
        }

        return accountResult.Value;
    }
}