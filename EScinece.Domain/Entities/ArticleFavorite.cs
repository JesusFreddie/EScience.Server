using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleFavorite : BaseEntity
{
    public Guid ArticleId { get; set; }
    public Guid AccountId { get; set; }
    
    public ArticleFavorite() { }


    public static ArticleFavorite Create(Guid articleId, Guid accountId)
    {
        return new ArticleFavorite
        {
            ArticleId = articleId,
            AccountId = accountId,
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}