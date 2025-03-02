using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("branch")]
public class ArticleBranchController(
    IArticleService articleService,
    IArticleBranchService articleBranchService,
    ILogger<ArticleBranchController> logger
    ) : ControllerBase
{
    [HttpPost("create")]
    public Task<ActionResult<ArticleBranchDto>> Create(string articleTitle)
    {
        throw new NotSupportedException();
    }

    [HttpGet("{accountName}/{articleTitle}/{branchName}")]
    public async Task<ActionResult<ArticleBranchDto>> Get(string branchName)
    {
        try
        {
            // var article = await articleBranchService.GetById(branchId);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}