using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Authorize]
[Route("account/{accountName}/article/{title}")]
public class AccountArticleController(
    IArticleService articleService,
    IAccountService accountService,
    ILogger<AccountArticleController> logger
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ArticleDto>> GetArticle(string accountName, string title)
    {
        try
        {
            var account = await accountService.GetByName(accountName);
            if (account is null)
                return NotFound();
    
            var article = await articleService.GetByTitle(title, account.Id);
            return Ok(article);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}