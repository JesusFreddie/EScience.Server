using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleParticipantService
{
    public Task<Result<ArticleParticipantDto, string>> Create(
        Guid accountId, 
        Guid articleId, 
        ArticlePermissionLevel permissionLevel = ArticlePermissionLevel.READER);
    public Task<ArticleParticipantDto?> GetById(Guid id);
    public Task<bool> Delete(Guid id);
    
    public Task<ArticlePermissionLevel> GetArticlePermissionLevelByIds(Guid accountId, Guid articleId);
}