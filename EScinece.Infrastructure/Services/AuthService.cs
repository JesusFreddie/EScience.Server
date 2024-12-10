using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Helpers;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class AuthService(
    IUserService userService, 
    IAccountService accountService, 
    IPasswordHasher passwordHasher, 
    IJwtProvider jwtProvider,
    ILogger<IAuthService> logger) : IAuthService
{
    private readonly IUserService _userService = userService;
    private readonly IAccountService _accountService = accountService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<IAuthService> _logger = logger;

    private static class ErrorMessage
    {
        public const string InvalidDataLogin = "Invalid data login";
        public const string InvalidPassword = "Invalid password";
        public const string InvalidEmail = "Invalid email";
        public const string InvalidUsername = "Invalid username";
    }

    public async Task<string> Login(AuthDto data)
    {
        var account = await _accountService.FindByEmail(data.Email);

        if (account == null)
            return ErrorMessage.InvalidDataLogin;

        var isVetifyPass = _passwordHasher.Verify(password: data.Password, data.Password);

        if (!isVetifyPass)
            return ErrorMessage.InvalidDataLogin;

        var token = _jwtProvider.GenerateToken(account.User);

        return "";
    }

    public async Task<Result<AccountDto, string>> Register(AuthDto data)
    {
        var validationResult = ValidateRegisterData(data);
        
        if (!string.IsNullOrWhiteSpace(validationResult))
            return validationResult;
        
        var user = await _userService.Create(new UserDto(
            Email: data.Email,
            Password: data.Password,
            Id: Guid.Empty
        ));

        if (!user.onSuccess)
            return user.Error;

        var accountResult = await _accountService.Create(new AccountDto(
            User: user.Value,
            Name: data.Name,
            Id: Guid.Empty,
            Role: Role.USER
        ));

        if (!accountResult.onSuccess)
        {
            await _userService.Delete(user.Value.Id);
            return accountResult.Error;
        }

        var account = accountResult.Value;

        return new AccountDto(
            Id: account.Id,
            User: account.User,
            Role: account.Role,
            Name: account.Name);
    }

    private string ValidateRegisterData(AuthDto data)
    {
        if (!IsValidEmail(data.Email))
        {
            _logger.LogWarning("Incorrect email: {Email}", data.Email);
            return ErrorMessage.InvalidEmail;
        }

        if (string.IsNullOrWhiteSpace(data.Password))
        {
            _logger.LogWarning("Incorrect password: {Password}", data.Password);
            return ErrorMessage.InvalidPassword;
        }

        if (string.IsNullOrWhiteSpace(data.Name))
        {
            _logger.LogWarning("Incorrect name: {Name}", data.Name);
            return ErrorMessage.InvalidUsername;
        }

        return "";
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}