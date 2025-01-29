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

    public async Task<Result<UserDto, string>> Create(string email, string password)
    {
        try
        {
            var existingUser = await _userRepository.GetByEmail(email);
            if (existingUser != null)
            {
                _logger.LogWarning("Попытка регистрации с существующим email: {Email}", email);
                return "Пользователь с таким email уже существует";
            }

            var hashedPassword = _passwordHasher.Generate(password);
            var result = User.Create(email, hashedPassword);
        
            if (!result.onSuccess)
            {
                _logger.LogError("Ошибка при создании пользователя: {Error}", result.Error);
                return result.Error;
            }
        
            var user = await _userRepository.Create(result.Value!);
            _logger.LogInformation("Успешно создан новый пользователь: {Email}", email);

            return new UserDto(
                Id: user.Id,
                Email: user.Email
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

    public async Task<UserDto?> GetById(Guid id)
    {
        var user = await _userRepository.GetById(id);
        if (user is null)
            return null;
        
        return new UserDto(
            Id: user.Id,
            Email: user.Email);
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user is null)
            return null;
        
        return new UserDto(
            Id: user.Id,
            Email: user.Email);
    }

    public async Task<Guid?> VerifyPass(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        
        if (user is null)
            return null;

        if (!_passwordHasher.Verify(password, user.HashedPassword))
            return null;
        
        return user.Id;
    }
}