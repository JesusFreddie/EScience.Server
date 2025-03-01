using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleBranchVersionService
{
    public Task<Result<ArticleBranchVersionDto, string>> Create(
        string text, Guid creatorId, Guid articleBranchId);
    
    public Task<ArticleBranchVersionDto?> GetById(Guid id);
}