using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction;

public interface IUserService
{
    public Task Register(UserRegusterDto request);
}