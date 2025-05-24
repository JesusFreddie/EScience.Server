using EScience.Application.Policy;
using EScience.Application.Requests;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Authorize]
[Route("{articleId}/version/{branchId:guid}")]
public class ArticleVersionController(
    ILogger<ArticleVersionController> logger,
    IArticleVersionService articleVersionService
    ) : ControllerBase
{
    [HttpGet("last", Name = "VersionGetLast")]
    [Authorize(Policy = ArticlePolicy.BranchReaderPolicy)]
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
    
    [HttpGet("first", Name = "VersionGetFist")]
    [Authorize(Policy = ArticlePolicy.BranchReaderPolicy)]
    public async Task<ActionResult<ArticleVersion>> GetFirst(Guid branchId)
    {
        try
        {
            var version = await articleVersionService.GetFirst(branchId);
            return Ok(version);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("save", Name = "VersionSave")]
    [Authorize(Policy = ArticlePolicy.BranchEditorBranchPolicy)]
    public async Task<IActionResult> Save(Guid branchId, [FromBody] SaveArticleTextRequest request)
    {
        try
        {
            var result = await articleVersionService.Save(branchId, request.Text);
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

    [HttpGet("get-all", Name = "VersionGetAllInfo")]
    [Authorize(Policy = ArticlePolicy.BranchReaderPolicy)]
    public async Task<ActionResult<IEnumerable<VersionInfo>>> GetAllInfo(Guid branchId)
    {
        try
        {
            var result = await articleVersionService.GetVersionInfo(branchId);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}