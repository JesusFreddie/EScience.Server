using System.ComponentModel.DataAnnotations.Schema;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScinece.Domain.Entities;

public class ArticleBranch: BaseEntity
{

    public string Name { get; set; }
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
    
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    
    public ICollection<ArticleBranchVersion> ArticleBranchVersions { get; set; } = new List<ArticleBranchVersion>();

    public ArticleBranch() {}
    
    private ArticleBranch(string name, Article article, ArticleParticipant creator)
    {
        Name = name;
        Article = article;
        Creator = creator;
    }

    public static Result<ArticleBranch?, string> Create(string name, Article article, ArticleParticipant creator)
    {
        if (string.IsNullOrEmpty(name))
            return ArticleBranchErrorMessage.NameIsRequired;

        return new ArticleBranch(name, article, creator);
    }
}