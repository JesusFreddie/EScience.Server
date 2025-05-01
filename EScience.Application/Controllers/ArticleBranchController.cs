using EScience.Application.Policy;
using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("branch/{articleId}")]
public class ArticleBranchController(
    IArticleBranchService articleBranchService,
    ILogger<ArticleBranchController> logger
    ) : ControllerBase
{
    [HttpPost("create", Name = "BranchCreate")]
    // [Authorize(Policy = ArticlePolicy.ArticleEditorPolicy)]
    public async Task<ActionResult<ArticleBranchDto>> Create(Guid articleId, [FromBody] CreateBranchDto req)
    {
        try
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == CustomClaims.AccountId);
            if (accountIdClaim is null || !Guid.TryParse(accountIdClaim.Value, out Guid accountId))
                return Unauthorized();

            var result = await articleBranchService.Create(req.Name, accountId, articleId, req.ParentId);
            if (!result.onSuccess)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{branchName}", Name = "BranchGet")]
    [Authorize(Policy = ArticlePolicy.ArticleReaderPolicy)]
    public async Task<ActionResult<ArticleBranch>> Get(Guid articleId, string branchName)
    {
        try
        {
            var result = await articleBranchService.GetByTitle(branchName, articleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet(Name = "BranchGetAll")]
    [Authorize(Policy = ArticlePolicy.ArticleReaderPolicy)]
    public async Task<ActionResult<IEnumerable<ArticleBranch>>> GetAll(Guid articleId)
    {
        try 
        {
            var result = await articleBranchService.GetAllByArticleId(articleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("id/{branchId}", Name = "BranchGetById")]
    public async Task<ActionResult<ArticleBranch>> GetById(Guid branchId)
    {
        try 
        {
            var result = await articleBranchService.GetById(branchId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}