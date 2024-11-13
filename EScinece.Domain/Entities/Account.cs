using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class Account: BaseEntity
{
    public string Name { get; set; }
    public Role Role { get; set; } = Role.USER;
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } =  new List<ArticleParticipant>();

    private Account(string name, User user, Role role = Role.USER)
    {
        Name = name;
        User = user;
        Role = role;
    }

    public static Result<Account, string> Create(string name, User user, Role role = Role.USER)
    {
        if (string.IsNullOrEmpty(name))
            return "Name is required";

        return new Account(name, user, role);
    }
}

public enum Role: byte
{
    ADMIN,
    USER
}