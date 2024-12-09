using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Helpers;
public interface IJwtProvider
{
    string GenerateToken(User user);
}