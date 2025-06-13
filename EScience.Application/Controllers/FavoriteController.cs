using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("favorite/{articleId:guid}")]
public class FavoriteController(
    ILogger<ArticleController> logger,
    IArticleFavoriteService favoriteService
    ) : ControllerBase
{
    [HttpPost("set", Name = "SetFavorite")]
    public async Task<IActionResult> SetFavorite(Guid articleId)
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();

            await favoriteService.SetFavorite(articleId, id);
            
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("remove", Name = "RemoveFavorite")]
    public async Task<IActionResult> RemoveFavorite(Guid articleId)
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();

            await favoriteService.RemoveFavorite(articleId, id);
            
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}