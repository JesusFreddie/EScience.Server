using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleBranchVersion: BaseEntity
{
    public string Text { get; set; }
    
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    
    public Guid ArticleBranchId { get; set; }
    public ArticleBranch ArticleBranch { get; set; }
}