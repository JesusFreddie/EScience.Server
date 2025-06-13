namespace EScinece.Domain.Abstraction.Services;

public interface IArticleFavoriteService
{
    public Task SetFavorite(Guid articleId, Guid accountId);
    public Task RemoveFavorite(Guid articleId, Guid accountId);
}