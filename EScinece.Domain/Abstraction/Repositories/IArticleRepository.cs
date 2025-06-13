using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleRepository : IRepository<Article>
{
    public Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id);
    public Task<IEnumerable<Article>> GetAllByAccountId(Guid id, int limit, int skip);
    public Task<IEnumerable<Article>> GetAllByArticleParticipantIdAndAccountId(Guid id);
    public Task<IEnumerable<Article>> GetAllByArticleFavoriteParticipantIdAndAccountId(Guid id);
    public Task<Article?> GetByTitle(string title, Guid accountId, string? branchName = null);
    public Task<int> GetCountByAccountId(Guid accountId);
    public Task<int> GetCountFavoriteByAccountId(Guid accountId);
    public Task<int> GetCountCraetedByAccountId(Guid accountId);
    public Task<int> GetCountParticipantByAccountId(Guid accountId);
}