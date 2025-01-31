using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository(IDbConnectionFactory connectionFactory) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAll()
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<User>("SELECT * FROM users");
    }

    public async Task Create(User user)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            """
            INSERT INTO users (id, email, hashed_password)
            VALUES (@Id, @Email, @HashedPassword)
            """, user);
    }

    public async Task<User?> GetByEmail(string email)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE email LIKE @email", new { email });
        return user;
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<User?> GetById(Guid id)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE id = @id", new { id });
    }

    public Task<User> Update(User entity)
    {
        throw new NotImplementedException();
    }
}