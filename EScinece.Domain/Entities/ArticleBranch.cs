using System.Text.Json.Serialization;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;

namespace EScinece.Domain.Entities;

public class ArticleBranch: BaseEntity
{

    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("articleId")]
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
    
    [JsonPropertyName("creatorId")]
    public Guid CreatorId { get; set; }
    public ArticleParticipant Creator { get; set; }
    
    public ICollection<ArticleVersion> ArticleBranchVersions { get; set; } = new List<ArticleVersion>();

    public ArticleBranch? ParentBranch { get; set; }
    [JsonPropertyName("parent_id")]
    public Guid? ParentId { get; set; }
    
    [JsonPropertyName("is_main")]
    public bool IsMain { get; set; }
    
    public ArticleBranch() {}
    
    private ArticleBranch(string name, Guid articleId, Guid creatorId, Guid? parentId = null, bool isMain = false)
    {
        Id = Guid.NewGuid();
        Name = name;
        ArticleId = articleId;
        CreatorId = creatorId;
        ParentId = parentId;
        IsMain = isMain;
    }

    public static Result<ArticleBranch, string> Create(string name, Guid articleId, Guid creatorId, Guid? parentId = null, bool isMain = false)
    {
        if (string.IsNullOrEmpty(name))
            return ArticleBranchErrorMessage.NameIsRequired;

        return new ArticleBranch(name, articleId, creatorId, parentId, isMain);
    }
}