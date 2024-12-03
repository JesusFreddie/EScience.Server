using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace EScinece.Infrastructure.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UserService> _logger;
    
    public UserService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    
    public async Task<Result<UserDto, string>> Create(UserDto userRegister)
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
        
            if (!result.IsOk)
            {
                _logger.LogError("Ошибка при создании пользователя: {Error}", result.Error);
                return result.Error;
            }
        
            await _userRepository.Create(result.Value!);
            _logger.LogInformation("Успешно создан новый пользователь: {Email}", userRegister.Email);

            return new UserDto(
                Id: result.Value.Id,
                Email: result.Value.Email
            );
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

}