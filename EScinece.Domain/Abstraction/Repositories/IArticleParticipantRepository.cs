using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleParticipantRepository : IRepository<ArticleParticipant>
{
    public Task<ArticlePermissionLevel> GetArticlePermissionLevelByIds(Guid accountId, Guid articleId);
}