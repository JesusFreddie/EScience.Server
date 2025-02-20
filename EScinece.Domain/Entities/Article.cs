using System.ComponentModel.DataAnnotations;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScinece.Domain.Entities;

public class Article: BaseEntity
{
    public const int MaxTitleLength = 150;
    public const int MinTitleLength = 3;
    
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; } =  false;
    
    public ArticleParticipant Creator { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } = new List<ArticleParticipant>();
    
    public ICollection<ArticleBranch> ArticleBranches { get; set; } = new List<ArticleBranch>();
    public Guid? TypeArticleId { get; set; }
    public ArticleType ArticleType { get; set; }

    public Article() { }
    
    private Article(string title, string description, Guid? typeArticleId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        TypeArticleId = typeArticleId;
    }

    public static Result<Article, string> Create(string title, string description, Guid? typeArticleId)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            return ArticleErrorMessage.TitleAndDescriptionCannotBeEmpty;
        
        return new Article(title, description, typeArticleId);
    }
}