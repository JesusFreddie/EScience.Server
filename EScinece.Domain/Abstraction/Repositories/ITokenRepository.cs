using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface ITokenRepository
{
    public Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken);
    public Task<RefreshToken?> GetRefreshToken(Guid accountId);
    public Task<Guid?> GetAccountIdByRefreshToken(string refreshToken);
}