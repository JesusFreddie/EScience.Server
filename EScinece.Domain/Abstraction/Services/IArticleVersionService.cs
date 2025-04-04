using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleVersionService
{
    public Task<Result<ArticleVersion, string>> Create(
        string text, Guid creatorId, Guid articleBranchId);
    
    public Task<ArticleVersion?> GetById(Guid id);
    public Task<ArticleVersion?> GetLast(Guid branchId);
    public Task<bool> Save(Guid branchId, string text);
}