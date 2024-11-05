using EScience.Application.Requests;

namespace EScinece.Domain.Abstraction;

public interface IUserService
{
    public Task Register(UserRequest request);
}