using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleBranchVersion: BaseEntity
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
    [JsonPropertyName("articleParticipantId")]
    public Guid ArticleParticipantId { get; set; }
    public ArticleParticipant ArticleParticipant { get; set; }
    [JsonPropertyName("articleBranchId")]
    public Guid ArticleBranchId { get; set; }
    public ArticleBranch ArticleBranch { get; set; }

    public ArticleBranchVersion() {}
    
    private ArticleBranchVersion(string text, Guid articleParticipantId, Guid articleBranchId)
    {
        Text = text;
        ArticleParticipantId = articleParticipantId;
        ArticleBranchId = articleBranchId;
    }

    public static Result<ArticleBranchVersion, string> Create(
        string text, Guid creatorId, Guid articleBranchId)
    {
        return new ArticleBranchVersion(text, creatorId, articleBranchId);
    }
    
}