using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

internal class UserRepository: IUserRepository
{
    public Task<User?> Create(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetById(Guid userId)
    {
        throw new NotImplementedException();
    }
}