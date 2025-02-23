using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IAuthService
{
    Task<TokensDto?> Login(string email, string password);
    Task<Result<AccountDto, string>> Register(string email, string password, string name);
    Task <TokensDto?> RefreshToken(string refreshToken);
}