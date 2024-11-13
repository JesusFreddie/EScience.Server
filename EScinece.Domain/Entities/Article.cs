using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class Article: BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } = new List<ArticleParticipant>();
    
    public ICollection<ArticleBranch> ArticleBranches { get; set; } = new List<ArticleBranch>();

    
    private Article(string title, string description, ArticleParticipant creator)
    {
        Title = title;
        Description = description;
        Creator = creator;
    }

    public static Result<Article?, string> Create(string title, string description, ArticleParticipant creator)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            return "Title and Description cannot be null or empty!";
        
        return new Article(title, description, creator);
    }
}