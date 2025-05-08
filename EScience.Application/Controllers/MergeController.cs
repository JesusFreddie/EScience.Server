using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("merge/{articleId:guid}/version/{branchId:guid}")]
public class MergeController(
    ILogger<MergeController> logger,
    IArticleVersionService articleVersionService,
    IArticleParticipantService articleParticipantService,
    IMergeService mergeService
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

    [HttpPost("merge-request", Name = "GetMergeRequest")]
    public async Task<ActionResult<List<TextDiff>>> GetMergeRequest(
        Guid branchId,
        [FromBody] GetMergeRequest req
        )
    {
        try
        {
            var original = await articleVersionService.GetLast(req.OriginalBranchId);
            if (original is null)
                return BadRequest();
            
            var modified = await articleVersionService.GetLast(branchId);
            if (modified is null)
                return BadRequest();
            
            return mergeService.GetAddedFragments(original.Text, modified.Text);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}