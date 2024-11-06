using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class Account: BaseEntity
{
    public string name { get; set; }
    public Role Role { get; set; } = Role.USER;
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } =  new List<ArticleParticipant>();
}

public enum Role: byte
{
    ADMIN,
    USER
}