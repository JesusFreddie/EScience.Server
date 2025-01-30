using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleRepository : IPaginations<Article>, IRepository<Article>
{
    public Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id);
    public Task<IEnumerable<Article>> GetAllByArticleParticipantIdInCreator(Guid id);
}