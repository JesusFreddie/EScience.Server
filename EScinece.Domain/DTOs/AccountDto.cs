using EScinece.Domain.Entities;

namespace EScinece.Domain.DTOs;

public record AccountDto(Guid Id, Guid UserId, Role Role, string Name);