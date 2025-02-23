namespace EScinece.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    // public string CreatedByIp { get; set; } TODO: может быть :)
    public DateTime? Revoked { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}