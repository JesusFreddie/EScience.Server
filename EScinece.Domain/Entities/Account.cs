using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScinece.Domain.Entities;

public class Account: BaseEntity
{
    public const int NameMaxLength = 50;
    public const int NameMinLength = 2;
    
    public string Name { get; set; }
    public Role Role { get; set; } = Role.USER;
 
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } =  new List<ArticleParticipant>();
    
    private Account(string name, Guid userId, Role role = Role.USER)
    {
        Name = name;
        UserId = userId;
        Role = role;
    }

    public Account() {}
    
    public static Result<Account, string> Create(string name, Guid userId, Role role = Role.USER)
    {
        if (string.IsNullOrEmpty(name))
            return AccountErrorMessage.NameIsRequired;

        return new Account(name, userId, role);
    }
}

public enum Role: int
{
    ADMIN,
    USER
}