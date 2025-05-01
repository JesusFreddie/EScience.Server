using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("merge/{articleId}/version/{branchId}")]
public class MergeController(
    ILogger<MergeController> logger,
    IArticleVersionService articleVersionService,
    IArticleParticipantService articleParticipantService
    ) : ControllerBase        
{
    [HttpPost(Name = "Merge")]
    public async Task<ActionResult<SuccessResponse>> MergeArticle(
        Guid articleId,
        Guid branchId,
        [FromBody] MergeRequest req
        )
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();

            var participant = await articleParticipantService.GetByAccount(id, articleId);

            if (participant is null)
                return BadRequest();
            
            var version = await articleVersionService.Create(
                text: req.Text,
                creatorId: participant.Id,
                articleBranchId: branchId
                );

            if (version.onSuccess)
                return Ok(new SuccessResponse(true));
            
            return BadRequest(new SuccessResponse(false));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}