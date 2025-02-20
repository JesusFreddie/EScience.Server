namespace EScinece.Domain.Entities;

public class Refresh
{
    public Guid AccountId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}