using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IAccountInfoService
{
    public Task<Result<AccountInfo, string>> CreateAsync(string field, string value, Guid accountId);
    public Task UpdateAsync(Guid id, string field, string value);
    public Task DeleteAsync(Guid id);
    public Task<AccountInfo?> GetAsync(Guid id);
    public Task<List<AccountInfo>> GetAllAsync(Guid accountId);
}