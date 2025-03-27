using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IAccountService
{
    public Task<Result<Account, string>> Create(Guid userId, string name);
    public Task<Account?> GetById(Guid id);
    public Task<Account?> GetByEmail(string email);
    public Task<Account?> GetByUserId(Guid id);
    public Task<ProfileDto?> GetProfile(string name);
    public Task<ProfileDto?> GetProfile(Guid accountId);
    public Task<Account?> GetByName(string name);
}
