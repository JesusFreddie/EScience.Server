using System.ComponentModel.DataAnnotations;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScinece.Domain.Entities;

public class User: BaseEntity
{
    public const int MaxPasswordLength = 100;
    public const int MinPasswordLength = 6;

    [Required]
    public string Email { get; private set; }
    
    [Required]
    public string HashedPassword { get; private set; }
    
    public Account? Account { get; set; }
    
    private User(string email, string hashedPassword)
    {
        Id = Guid.NewGuid();
        Email = email;
        HashedPassword = hashedPassword;
    }

    public static Result<User?, string> Create(string email, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return AuthErrorMessage.EmailIsRequired;
        }

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return AuthErrorMessage.PasswordIsRequired;
        }
        
        return new User(email.Trim(), hashedPassword);
    }
}