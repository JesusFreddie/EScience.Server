using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction;

public interface IAuthService
{
    Task<Result<UserDto, string>> Login(UserDto user);
    Task<Result<UserDto, string>> Register(RegisterDto data);
}