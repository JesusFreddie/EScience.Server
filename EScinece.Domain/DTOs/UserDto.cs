namespace EScinece.Domain.DTOs;

public record UserDto(Guid Id, string Email, string PasswordHash);