using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleParticipantRepository : IRepository<ArticleParticipant>
{
    public Task<ArticlePermissionLevel> GetArticlePermissionLevelByAccountId(Guid id);
    public Task<ArticlePermissionLevel> GetArticlePermissionLevelById(Guid id);
}