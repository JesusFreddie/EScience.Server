using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IUserService
{
    public Task<Result<User, string>> Create(UserDto user);
    public Task<User?> FindById(string id);
    public Task<User?> FindByEmail(string email);
    public Task<bool> Delete(Guid id);
}