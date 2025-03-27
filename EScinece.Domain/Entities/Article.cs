using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScinece.Domain.Entities;

public class Article: BaseEntity
{
    public const int MaxTitleLength = 150;
    public const int MinTitleLength = 3;
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; set; } =  false;
    
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    
    public ICollection<ArticleParticipant> ArticleParticipants { get; set; } = new List<ArticleParticipant>();
    
    public ICollection<ArticleBranch> ArticleBranches { get; set; } = new List<ArticleBranch>();
    
    [JsonPropertyName("typeArticleId")]
    public Guid? TypeArticleId { get; set; }
    public ArticleType ArticleType { get; set; }

    public Article() { }
    
    private Article(string title, string description, Guid accountId, bool isPrivate, Guid? typeArticleId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        TypeArticleId = typeArticleId;
        IsPrivate = isPrivate;
        AccountId = accountId;
    }

    public static Result<Article, string> Create(string title, string description, Guid accountId, bool isPrivate, Guid? typeArticleId)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            return ArticleErrorMessage.TitleAndDescriptionCannotBeEmpty;
        
        return new Article(title, description, accountId, isPrivate, typeArticleId);
    }
}