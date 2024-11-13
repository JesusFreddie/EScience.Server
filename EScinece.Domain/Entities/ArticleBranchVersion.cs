using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleBranchVersion: BaseEntity
{
    public string? Text { get; set; }
    
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    
    public Guid ArticleBranchId { get; set; }
    public ArticleBranch ArticleBranch { get; set; }

    private ArticleBranchVersion(string? text, ArticleParticipant creator, ArticleBranch articleBranch)
    {
        Text = text;
        Creator = creator;
        ArticleBranch = articleBranch;
    }

    public static Result<ArticleBranchVersion, string> Create(string? text, ArticleParticipant creator,
        ArticleBranch articleBranch)
    {
        return new ArticleBranchVersion(text, creator, articleBranch);
    }
    
}