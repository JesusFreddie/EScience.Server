using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleParticipantService
{
    public Task<Result<ArticleParticipantDto, string>> Create(Guid accountId, Guid articleId);
    public Task<ArticleParticipantDto?> GetById(Guid id);
    public Task<bool> Delete(Guid id);
}