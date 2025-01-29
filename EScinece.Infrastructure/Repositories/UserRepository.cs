using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository(EScienceDbContext context) : IUserRepository
{
    public async Task<User> Create(User user)
    {
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await context
            .User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await context.User.FindAsync(id);
        if (user is null)
            return false;
        
        context.User.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<User?> GetById(Guid id)
    {
        return await context.User.FindAsync(id);
    }

    public Task<List<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    

    public Task<User> Update(User entity)
    {
        throw new NotImplementedException();
    }
}