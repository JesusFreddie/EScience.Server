using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleBranchVersion: BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    public Guid ArticleBranchId { get; set; }
    public ArticleBranch ArticleBranch { get; set; }

    public ArticleBranchVersion() {}
    
    private ArticleBranchVersion(string text, Guid creatorId, Guid articleBranchId)
    {
        Text = text;
        CreatorId = creatorId;
        ArticleBranchId = articleBranchId;
    }

    public static Result<ArticleBranchVersion, string> Create(
        string text, Guid creatorId, Guid articleBranchId)
    {
        return new ArticleBranchVersion(text, creatorId, articleBranchId);
    }
    
}