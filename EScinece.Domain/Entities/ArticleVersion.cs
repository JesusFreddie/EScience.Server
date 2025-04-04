using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleVersion: BaseEntity
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
    [JsonPropertyName("articleParticipantId")]
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    [JsonPropertyName("articleBranchId")]
    public Guid ArticleBranchId { get; set; }
    public ArticleBranch ArticleBranch { get; set; }

    public ArticleVersion() {}
    
    private ArticleVersion(string text, Guid creatorId, Guid articleBranchId)
    {
        Id = Guid.NewGuid();
        Text = text;
        CreatorId = creatorId;
        ArticleBranchId = articleBranchId;
    }

    public static Result<ArticleVersion, string> Create(
        string text, Guid creatorId, Guid articleBranchId)
    {
        return new ArticleVersion(text, creatorId, articleBranchId);
    }
    
}