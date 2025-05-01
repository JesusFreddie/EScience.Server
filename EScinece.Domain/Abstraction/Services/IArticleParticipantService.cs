using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleParticipantService
{
    public Task<Result<ArticleParticipant, string>> Create(
        Guid accountId, 
        Guid articleId, 
        ArticlePermissionLevel permissionLevel = ArticlePermissionLevel.READER);
    public Task<ArticleParticipant?> GetById(Guid id);
    public Task<bool> Delete(Guid id);
    public Task<ArticleParticipant?> GetByAccount(Guid accountId, Guid articleId);
    
    public Task<ArticlePermissionLevel> GetArticlePermissionLevelByIds(Guid accountId, Guid articleId);
}