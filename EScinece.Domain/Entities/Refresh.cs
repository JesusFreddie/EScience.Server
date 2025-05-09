using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class Refresh : BaseEntity
{
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("expires")]
    public DateTime Expires { get; set; }
}