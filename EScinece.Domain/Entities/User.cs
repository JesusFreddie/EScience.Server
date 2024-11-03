using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class User: BaseEntity
{
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}