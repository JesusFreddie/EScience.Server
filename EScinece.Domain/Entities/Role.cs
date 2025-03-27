using System.Text.Json.Serialization;

namespace EScinece.Domain.Entities;

public class Role
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    public Role() {}
}