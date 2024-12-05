using EScinece.Domain.Entities;

namespace EScinece.Domain.DTOs;

public record AccountDto(Guid? Id, User User, Role Role, string Name);