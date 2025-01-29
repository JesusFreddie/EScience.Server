using System.ComponentModel.DataAnnotations;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Entities;

namespace EScience.Application.Requests;

public record struct RegisterRequestDto(
    [Required(ErrorMessage = AuthErrorMessage.EmailIsRequired)]
    [EmailAddress(ErrorMessage = AuthErrorMessage.InvalidEmail)]
    string Email, 
    
    [Required(ErrorMessage = AuthErrorMessage.PasswordIsRequired)]
    [StringLength(User.MaxPasswordLength, ErrorMessage = AuthErrorMessage.PasswordIsRequired, MinimumLength = User.MinPasswordLength)]
    string Password,
    
    [Required(ErrorMessage = AuthErrorMessage.NameIsRequired)]
    string Name
);