using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction;

public interface IUserRepository
{
    public Task<User?> Create(User user);
    public Task<User?> GetByEmail(string email);
}