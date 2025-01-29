using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IAuthService
{
    Task<string> Login(string email, string password);
    Task<Result<AccountDto, string>> Register(string email, string password, string name);
}