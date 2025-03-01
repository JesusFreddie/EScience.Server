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
    
    private ArticleBranch(string name, Guid articleId, Guid creatorId)
    {
        Name = name;
        ArticleId = articleId;
        CreatorId = creatorId;
    }

    public static Result<ArticleBranch, string> Create(string name, Guid articleId, Guid creatorId)
    {
        if (string.IsNullOrEmpty(name))
            return ArticleBranchErrorMessage.NameIsRequired;

        return new ArticleBranch(name, articleId, creatorId);
    }
}