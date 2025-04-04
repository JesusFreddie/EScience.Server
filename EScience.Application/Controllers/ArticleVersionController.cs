using EScience.Application.Policy;
using EScience.Application.Requests;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Authorize]
[Route("{articleId}/version/{branchId}")]
public class ArticleVersionController(
    ILogger<ArticleVersionController> logger,
    IArticleVersionService articleVersionService
    ) : ControllerBase
{
    [HttpGet("last")]
    public async Task<ActionResult<ArticleVersion>> GetLast(Guid branchId)
    {
        try
        {
            var version = await articleVersionService.GetLast(branchId);
            return Ok(version);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] SaveArticleTextRequest request)
    {
        try
        {
            var result = await articleVersionService.Save(request.BranchId, request.Text);
            if (!result)
                return BadRequest();
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}