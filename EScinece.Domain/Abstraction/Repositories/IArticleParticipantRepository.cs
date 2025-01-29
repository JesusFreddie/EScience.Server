using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleParticipantRepository
{
    public Task<ArticleParticipant> Create(ArticleParticipant articleParticipant);
    public Task<ArticleParticipant?> GetById(Guid id);
    public Task<Guid> Delete(Guid id);
}