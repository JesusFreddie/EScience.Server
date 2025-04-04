using EScience.Application.Policy;
using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("branch")]
public class ArticleBranchController(
    IArticleBranchService articleBranchService,
    ILogger<ArticleBranchController> logger
    ) : ControllerBase
{
    [HttpPost("create")]
    [Authorize(Policy = ArticlePolicy.ArticleEditorPolicy)]
    public Task<ActionResult<ArticleBranchDto>> Create(string articleTitle)
    {
        throw new NotSupportedException();
    }

    [HttpGet("{articleId}/{branchName}")]
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

    [HttpGet("{articleId}")]
    public async Task<ActionResult<List<ArticleBranch>>> GetAll(Guid articleId)
    {
        try
        {
            throw new Exception();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    
}