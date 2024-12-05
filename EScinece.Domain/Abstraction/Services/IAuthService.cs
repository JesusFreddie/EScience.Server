using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IAuthService
{
    Task<string> Login(AuthDto data);
    Task<Result<AccountDto, string>> Register(AuthDto data);
}