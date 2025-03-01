using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction.Services;

public interface IAccountService
{
    public Task<Result<AccountDto, string>> Create(Guid userId, string name);
    public Task<AccountDto?> GetById(Guid id);
    public Task<AccountDto?> GetByEmail(string email);
    public Task<AccountDto?> GetByUserId(Guid id);
    public Task<ProfileDto?> GetProfile(Guid accountId);
}
