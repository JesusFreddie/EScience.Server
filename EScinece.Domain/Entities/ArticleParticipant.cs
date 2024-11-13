using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleParticipant: BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public bool IsAccepted { get; set; } = false;
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
    
    public ICollection<ArticleBranch> ArticleBranches { get; set; } =  new List<ArticleBranch>();
    public ICollection<ArticleBranchVersion> ArticleBranchVersions { get; set; } = new List<ArticleBranchVersion>();

    private ArticleParticipant(Account account, Article article)
    {
        Account = account;
        Article = article;
    }

    public static Result<ArticleParticipant, string> Create(Account account, Article article)
    {
        return new ArticleParticipant(account, article);
    }
}