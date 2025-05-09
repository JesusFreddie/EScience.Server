using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class Notification
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("account_id")]
    public Guid AccountId { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    [JsonPropertyName("type")]
    public NotificationType Type { get; set; }
    [JsonPropertyName("is_read")]
    public bool IsRead { get; set; }
    [JsonPropertyName("readed_date")]
    public DateTime ReadedDate { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

public enum NotificationType
{
    Information = 1,
    Message,
    Invite
}