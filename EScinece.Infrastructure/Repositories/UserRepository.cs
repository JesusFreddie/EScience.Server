using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly EScienceDbContext _context;
    
    public UserRepository(EScienceDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> Create(User user)
    {
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<User?> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }
}