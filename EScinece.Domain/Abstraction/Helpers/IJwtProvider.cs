using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Helpers;
public interface IJwtProvider
{
    string GenerateToken(Guid userId);
}