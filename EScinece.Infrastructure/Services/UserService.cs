using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Helpers;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ILogger<UserService> logger
    ) : IUserService
{
    
    private readonly ILogger<UserService> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<User, string>> Create(UserDto userRegister)
    {
        try
        {
            if (!IsValidEmail(userRegister.Email))
            {
                _logger.LogWarning("Попытка регистрации с некорректным email: {Email}", userRegister.Email);
                return "Некорректный формат email";
            }

            var existingUser = await _userRepository.GetByEmail(userRegister.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Попытка регистрации с существующим email: {Email}", userRegister.Email);
                return "Пользователь с таким email уже существует";
            }

            var hashedPassword = _passwordHasher.Generate(userRegister.Password);
            var result = User.Create(userRegister.Email, hashedPassword);
        
            if (!result.onSuccess)
            {
                _logger.LogError("Ошибка при создании пользователя: {Error}", result.Error);
                return result.Error;
            }
        
            var user = await _userRepository.Create(result.Value!);
            _logger.LogInformation("Успешно создан новый пользователь: {Email}", userRegister.Email);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при создании пользователя");
            return "Произошла внутренняя ошибка сервера";
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.Delete(id);
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

    public Task<User?> FindById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> FindByEmail(string email)
    {
        throw new NotImplementedException();
    }
}