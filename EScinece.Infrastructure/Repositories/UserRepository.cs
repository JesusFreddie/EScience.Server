using System.Data;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository(IDbConnectionFactory connectionFactory, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAll()
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<User>("SELECT * FROM users");
    }

    public async Task Create(User user)
    {
        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO users (id, email, hashed_password)
                VALUES (@Id, @Email, @HashedPassword)
                """, user);
        }
        catch (NpgsqlException ex)
        {
            logger.LogError("Произошла ошибка sql зпроса создания user", ex);
            throw new Exception(ex.Message, ex);
        }
        catch (DbConnectionException ex)
        {
            throw new Exception(ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Произошла ошибка при создании user", ex);
            throw new Exception("Произошла ошибка при создании user", ex);
        }
    }

    public async Task<User?> GetByEmail(string email)
    {
        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE email = @email", new { email });
            return user;
        }
        catch (DbConnectionException ex)
        {
            throw new Exception(ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Произошла ошибка при получении пользователей по email", ex);
            throw new Exception("Произошла ошибка при получении пользователей по email", ex);
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<User?> GetById(Guid id)
    {
        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @id", new { id });
        }
        catch (DbConnectionException ex)
        {
            throw new Exception(ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Произошла ошибка при получении пользователей по id", ex);
            throw new Exception("Произошла ошибка при получении пользователей по id", ex);
        }
    }

    public Task<User> Update(User entity)
    {
        throw new NotImplementedException();
    }
}