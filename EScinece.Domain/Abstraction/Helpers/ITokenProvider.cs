using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Helpers;
public interface ITokenProvider
{
    Task<TokensDto> GenerateToken(Guid accountId);
    Task<Guid?> GetAccountIdByRefreshToken(string refreshToken);
}