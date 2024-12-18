using System.ComponentModel.DataAnnotations;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class User: BaseEntity
{
    private static class ErrorMessages
    {
        public const string EmailRequired = "Email обязателен для заполнения";
        public const string PasswordRequired = "Пароль обязателен для заполнения";
    }

    [Required]
    public string Email { get; private set; }
    [Required]
    public string HashedPassword { get; private set; }
    public Account? Account { get; set; }
    public Guid AccountId { get; set; }
    
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
            return ErrorMessages.EmailRequired;
        }

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return ErrorMessages.PasswordRequired;
        }
        
        return new User(email.Trim(), hashedPassword);
    }
}