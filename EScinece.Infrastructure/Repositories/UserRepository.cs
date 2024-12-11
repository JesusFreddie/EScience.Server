using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository(EScienceDbContext context) : IUserRepository
{
    public async Task<User> Create(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}