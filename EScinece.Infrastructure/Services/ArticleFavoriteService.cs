using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleFavoriteService(
    ILogger<ArticleFavoriteService> logger,
    IArticleFavoriteRepository articleFavoriteRepository
    ) : IArticleFavoriteService
{
    public async Task SetFavorite(Guid articleId, Guid accountId)
    {
        try
        {
            var favorite = ArticleFavorite.Create(articleId, accountId);
            await articleFavoriteRepository.SetFavorite(favorite);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task RemoveFavorite(Guid articleId, Guid accountId)
    {
        try
        {
            await articleFavoriteRepository.RemoveFavorite(articleId, accountId);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
    }
}