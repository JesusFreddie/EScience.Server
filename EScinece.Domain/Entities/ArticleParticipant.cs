using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;

namespace EScinece.Domain.Entities;

public class ArticleParticipant: BaseEntity
{
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public bool IsAccepted { get; set; } = false;
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }

    public ArticlePermissionLevel PermissionLevel { get; set; }
    
    public ICollection<ArticleBranch> ArticleBranches { get; set; } =  new List<ArticleBranch>();
    public ICollection<ArticleVersion> ArticleBranchVersions { get; set; } = new List<ArticleVersion>();

    public ArticleParticipant() {}
    
    private ArticleParticipant(
        Guid accountId, 
        Guid articleId, 
        ArticlePermissionLevel permissionLevel = ArticlePermissionLevel.READER,
        bool isAccepted = false
        )
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        ArticleId = articleId;
        PermissionLevel = permissionLevel;
        IsAccepted = isAccepted;
    }

    public static Result<ArticleParticipant, string> Create(
        Guid accountId, 
        Guid articleId, 
        ArticlePermissionLevel permissionLevel = ArticlePermissionLevel.READER,
        bool isAccepted = false
        )
    {
        return new ArticleParticipant(accountId, articleId, permissionLevel, isAccepted);
    }
}