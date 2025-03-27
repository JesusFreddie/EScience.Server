using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleBranchService
{
    public Task<Result<ArticleBranch, string>> Create(
        string name, Guid creatorId, Guid articleId);
    public Task<ArticleBranch?> GetById(Guid id);
    public Task<ArticleBranch?> GetByTitle(string name, Guid articleId);
}