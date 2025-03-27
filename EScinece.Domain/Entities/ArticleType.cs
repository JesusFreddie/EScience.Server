using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleType : BaseEntity
{
    public ArticleType() {}
    [JsonPropertyName("name")]
    public string Name { get; set; }
}