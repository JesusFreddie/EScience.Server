using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleFavoriteRepository
{
    public Task SetFavorite(ArticleFavorite articleFavorite);
    public Task RemoveFavorite(Guid articleId, Guid accountId);
}