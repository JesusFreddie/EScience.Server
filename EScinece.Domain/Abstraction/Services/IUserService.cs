using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IUserService
{
    public Task<Result<UserDto, string>> Create(string email, string password);
    public Task<UserDto?> GetById(Guid id);
    public Task<UserDto?> GetByEmail(string email);
    public Task<bool> Delete(Guid id);
    public Task<Guid?> VerifyPass(string email, string password);
}