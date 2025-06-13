using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("permision")]
public class ArticlePermisionController(
    ILogger<ArticlePermisionController> logger,
    IArticleParticipantService articleParticipantService
    ) : ControllerBase
{
    [HttpGet("article/{articleId:guid}", Name = "PermisionGet")]
    public async Task<ActionResult<ArticlePermissionLevel>> GetPermisionAsync(Guid articleId)
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            
            var result = await articleParticipantService.GetArticlePermissionLevelByIds(id, articleId);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}