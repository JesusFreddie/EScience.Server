using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("branch")]
public class ArticleBranchController(
    IAccountService accountService,
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
    public async Task<ActionResult<ArticleBranch>> Get(string accountName, string articleTitle, string branchName)
    {
        try
        {
            var account = await accountService.GetByName(accountName);
            if (account == null)
                return NotFound();
            var article = await articleService.GetByTitle(articleTitle, account.Id);
            if (article == null)
                return NotFound();
            var articleBranch = await articleBranchService.GetByTitle(articleTitle, article.Id);
            return Ok(articleBranch);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}