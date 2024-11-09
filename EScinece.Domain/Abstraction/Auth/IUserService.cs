using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction;

public interface IUserService
{
    public Task<Result<User?, Exception>> Register(UserRegusterDto request);
}