using System.ComponentModel.DataAnnotations;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScience.Application.Requests;

public record LoginRequestDto(
    [Required(ErrorMessage = AuthErrorMessage.InvalidEmail)] string Email,
    [Required(ErrorMessage = AuthErrorMessage.PasswordIsRequired)] string Password
);