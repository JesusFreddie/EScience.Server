using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class Article: BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public Guid CreatorId { get; set; }
    public ArticleParticipant? Creator { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } = new List<ArticleParticipant>();
    
    public ICollection<ArticleBranch> ArticleBranches { get; set; } = new List<ArticleBranch>();
}