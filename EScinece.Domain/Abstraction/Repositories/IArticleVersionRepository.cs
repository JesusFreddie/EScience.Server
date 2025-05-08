using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleVersionRepository : IRepository<ArticleVersion>
{
    public Task<ArticleVersion?> GetLatestVersion(Guid branchId);
    public Task<ArticleVersion?> GetFirstVersion(Guid branchId);
    public Task<bool> SaveText(Guid branchId, string text);
}