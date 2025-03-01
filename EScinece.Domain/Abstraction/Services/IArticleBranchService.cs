using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleBranchService
{
    public Task<Result<ArticleBranchDto, string>> Create(
        string name, Guid creatorId, Guid articleId);
    public Task<ArticleBranchDto?> GetById(Guid id);
}