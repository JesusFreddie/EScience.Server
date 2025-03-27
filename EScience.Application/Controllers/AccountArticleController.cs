using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[AllowAnonymous]
[Route("/account/{accountName}/article/{articleName}")]
public class AccountArticleController(
    IArticleService articleService,
    IAccountService accountService,
    ILogger<AccountArticleController> logger
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Article>> GetArticle(string accountName, string articleName)
    {
        try
        {
            var account = await accountService.GetByName(accountName);
            if (account is null)
                return NotFound();
     
            var article = await articleService.GetByTitle(articleName, account.Id);
            return Ok(article);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}