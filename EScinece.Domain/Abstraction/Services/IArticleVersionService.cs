using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleVersionService
{
    public Task<Result<ArticleVersion, string>> Create(
        string text, Guid creatorId, Guid articleBranchId);

    public Task<Result<ArticleVersion, string>> Copy(Guid versionId, Guid? creatorId = null, Guid? articleBranchId = null);
    public Task<Result<ArticleVersion, string>> CopyByBranch(Guid branchId, Guid? creatorId = null, Guid? articleBranchId = null);
    
    public Task<ArticleVersion?> GetById(Guid id);
    public Task<ArticleVersion?> GetLast(Guid branchId);
    public Task<ArticleVersion?> GetFirst(Guid branchId);
    public Task<bool> Save(Guid branchId, string text, Guid? creatorId = null);
    public Task<IEnumerable<VersionInfo>> GetVersionInfo(Guid branchId);

}