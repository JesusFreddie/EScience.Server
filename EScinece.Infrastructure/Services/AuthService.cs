using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
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
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<IAuthService> _logger = logger;

    public async Task<string> Login(string email, string password)
    {
        var userId = await _userService.VerifyPass(email, password);
        
        if (userId is null)
            return string.Empty;
        
        var token = _jwtProvider.GenerateToken(userId.Value);

        return token;
    }

    public async Task<Result<AccountDto, string>> Register(string email, string password, string name)
    {
        var validationResult = ValidateRegisterData(email, password, name);
        
        if (!string.IsNullOrWhiteSpace(validationResult))
            return validationResult;

        var user = await _userService.Create(email, password);
        
        if (!user.onSuccess)
            return user.Error;
        
        var accountResult = await _accountService.Create(user.Value.Id, name);

        if (!accountResult.onSuccess)
        {
            await _userService.Delete(user.Value.Id);
            return accountResult.Error;
        }

        return accountResult.Value;
    }

    private string ValidateRegisterData(string email, string password, string name)
    {
        if (!IsValidEmail(email))
        {
            _logger.LogWarning("Incorrect email: {Email}", email);
            return AuthErrorMessage.InvalidEmail;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Incorrect password: {Password}", password);
            return AuthErrorMessage.PasswordIsRequired;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            _logger.LogWarning("Incorrect name: {Name}", name);
            return AuthErrorMessage.NameIsRequired;
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