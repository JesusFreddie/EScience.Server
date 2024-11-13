using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class User: BaseEntity
{
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    
    public Account? Account { get; set; }
    
    private User(string email, string hashedPassword)
    {
        Email = email;
        HashedPassword = hashedPassword; 
    }

    public static Result<User?, string> Create(string email, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return"Email is required";
        }

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return"Hashed password is required";
        }
        
        return new User(email, hashedPassword);
    }
}