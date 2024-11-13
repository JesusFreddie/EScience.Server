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

    private ArticleBranch(string name, Article article, ArticleParticipant creator)
    {
        Name = name;
        Article = article;
        Creator = creator;
    }

    public static Result<ArticleBranch?, string> Create(string name, Article article, ArticleParticipant creator)
    {
        if (string.IsNullOrEmpty(name))
            return "Name cannot be null or empty.";

        return new ArticleBranch(name, article, creator);
    }
}