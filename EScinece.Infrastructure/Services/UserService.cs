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

    public async Task<Result<UserDto, string>> Create(string email, string password)
    {
        try
        {
            var existingUser = await userRepository.GetByEmail(email);
            if (existingUser != null)
            {
                logger.LogWarning("Попытка регистрации с существующим email: {Email}", email);
                return "Пользователь с таким email уже существует";
            }

            var hashedPassword = passwordHasher.Generate(password);
            var result = User.Create(email, hashedPassword);
        
            if (!result.onSuccess && result.Value is null)
            {
                logger.LogWarning("Ошибка при создании пользователя: {Error}", result.Error);
                return result.Error;
            }
        
            await userRepository.Create(result.Value!);
            logger.LogInformation("Успешно создан новый пользователь: {Email}", email);

            return new UserDto(
                Id: result.Value!.Id,
                Email: email
                );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Непредвиденная ошибка при создании пользователя");
            return "Произошла внутренняя ошибка сервера";
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            return await userRepository.Delete(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка удаления пользователя");
            throw new Exception("Произошла серверная ошибка удаления пользователя");
        }
    }

    public async Task<UserDto?> GetById(Guid id)
    {
        try
        {
            var user = await userRepository.GetById(id);
            if (user is null)
                return null;
        
            return new UserDto(
                Id: user.Id,
                Email: user.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка получения пользователя");
            throw new Exception("Произошла серверная ошибка получения пользователя");
        }
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        try
        {
            var user = await userRepository.GetByEmail(email);
            if (user is null)
                return null;
        
            return new UserDto(
                Id: user.Id,
                Email: user.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка получения пользователя");
            throw new Exception("Произошла серверная ошибка получения пользователя");
        }
    }

    public async Task<Guid?> VerifyPass(string email, string password)
    {
        try
        {
            var user = await userRepository.GetByEmail(email);
        
            if (user is null)
                return null;

            if (!passwordHasher.Verify(password, user.HashedPassword))
                return null;
        
            return user.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка проверки пароля");
            throw new Exception("Произошла ошибка проверки пароля");
        }
    }
}