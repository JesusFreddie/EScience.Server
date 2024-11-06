using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    
    public UserRepository(DbContext context, IMapper mapper)
    {
        
    }
    
    public Task<User?> Create(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetById(Guid userId)
    {
        throw new NotImplementedException();
    }
}