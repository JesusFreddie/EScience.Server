using System.Text.Json.Serialization;

namespace EScinece.Domain.Entities;

public class Refresh
{
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("expires")]
    public DateTime Expires { get; set; }
}