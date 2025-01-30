using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByEmail(string email);
}