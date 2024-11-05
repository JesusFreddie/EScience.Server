using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleBranch: BaseEntity
{
    public string Name { get; set; }
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
    
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    
    public ICollection<ArticleBranchVersion> ArticleBranchVersions { get; set; }
}