using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Services;

public class AccountInfoService(
    IAccountInfoRepository accountInfoRepository
    ) : IAccountInfoService
{
    public async Task<Result<AccountInfo, string>> CreateAsync(string field, string value, Guid accountId)
    {
        var accountInfo = AccountInfo.Create(field, value, accountId);
        if (!accountInfo.onSuccess)
            return accountInfo.Error;

        await accountInfoRepository.SaveAsync(accountInfo.Value);
        
        return accountInfo.Value;
    }

    public async Task UpdateAsync(Guid id, string field, string value)
    {
        var accountInfo = await accountInfoRepository.GetAsync(id);

        if (accountInfo == null)
            return;
        
        accountInfo.Value = value;
        
        await accountInfoRepository.UpdateAsync(accountInfo);
    }

    public async Task DeleteAsync(Guid id)
    {
        await accountInfoRepository.DeleteAsync(id);
    }

    public async Task<AccountInfo?> GetAsync(Guid id)
    {
        return await accountInfoRepository.GetAsync(id);
    }

    public async Task<List<AccountInfo>> GetAllAsync(Guid accountId)
    {
        return await accountInfoRepository.GetAllByAccountAsync(accountId);
    }
}