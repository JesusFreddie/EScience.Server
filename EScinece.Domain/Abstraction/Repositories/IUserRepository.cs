using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IUserRepository
{
    public Task<User> Create(User user);
    public Task<User?> GetByEmail(string email);
    public Task<bool> Delete(Guid id);
}