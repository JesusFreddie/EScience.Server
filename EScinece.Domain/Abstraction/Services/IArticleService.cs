using EScinece.Domain.DTOs;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleService
{
    public Task<Result<ArticleDto, string>> Create(string title, string description, Guid accountId, Guid typeArticleId);
    public Task<ICollection<ArticleDto>> GetAllByArticleParticipantId(Guid id);
    public Task<ICollection<ArticleDto>> GetAllByArticleParticipantIdInCreator(Guid id);
    public Task<ArticleDto?> GetById(Guid id);
}