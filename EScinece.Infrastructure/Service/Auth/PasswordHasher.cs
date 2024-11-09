using EScinece.Domain.Abstraction;

namespace EScinece.Infrastructure.Service;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool Verify(string password, string hash)
    {   
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}