using EScience.Application.Policy;
using EScience.Application.Requests;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("participant/{articleId:guid}")]
public class ArticleParticipantController(
    ILogger<ArticleParticipantController> logger,
    IArticleParticipantService articleParticipantService
    ) : ControllerBase
{
    [HttpGet(Name = "ParticipantGetAllByArticle")]
    [Authorize(Policy = ArticlePolicy.ArticleReaderPolicy)]
    public async Task<ActionResult<IEnumerable<ArticleParticipant>>> GetAllByArticle(Guid articleId)
    {
        try
        {
            var result = await articleParticipantService.GetAllByArticle(articleId);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("invite", Name = "ParticipantInvite")]
    [Authorize(Policy = ArticlePolicy.ArticleReaderPolicy)]
    public async Task<ActionResult> Invite(
        Guid articleId,
        [FromBody] ParticipantInviteRequest req,
        [FromServices] IAccountService accountService
        )
    {
        try
        {
            var account  = await accountService.GetByEmail(req.Email);
            if (account is null)
                return Ok();
            
            await articleParticipantService.Create(account.Id, articleId, req.PermissionLevel);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}