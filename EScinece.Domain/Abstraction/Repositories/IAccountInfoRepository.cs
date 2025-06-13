using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IAccountInfoRepository
{
    public Task<AccountInfo?> GetAsync(Guid id);
    public Task<List<AccountInfo>> GetAllByAccountAsync(Guid accountId);
    public Task<bool> SaveAsync(AccountInfo accountInfo);
    public Task<bool> DeleteAsync(Guid id);
    public Task<bool> UpdateAsync(AccountInfo accountInfo);
}