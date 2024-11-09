namespace EScinece.Domain.Abstraction;

public interface IPasswordHasher
{
    public string Generate(string password);
    public bool Verify(string password, string hash);
}