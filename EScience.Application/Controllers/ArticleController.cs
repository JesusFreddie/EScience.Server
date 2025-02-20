using EScience.Application.Policy;
using EScience.Application.Requests;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Authorize]
[Route("articles")]
public class ArticleController(
    IArticleService articleService,
    IArticleParticipantService articleParticipantService,
    ILogger<ArticleController> logger) : ControllerBase
{
    
    [HttpPost("create")]
    public async Task<ActionResult<ArticleDto>> Create([FromBody] CreateArticleRequest request)
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            
            var article = await articleService.Create(
                title: request.Title,
                description: request.Description,
                accountId: id,
                typeArticleId: request.ArticleTypeId);
            
            return !article.onSuccess ? BadRequest(article.Error) : StatusCode(201, article.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{articleId}")]
    [Authorize(Policy = ArticlePolicy.ArticleReaderPolicy)]
    public async Task<ActionResult<ArticleDto?>> GetById(Guid articleId)
    {
        try
        {
            return await articleService.GetById(articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{articleId}")]
    [Authorize(Policy = ArticlePolicy.ArticleAuthorPolicy)]
    public async Task<IActionResult> SetParticipantForArticle(Guid articleId, [FromBody] SetParticipantRequest request)
    {
        try
        {
            await articleParticipantService.Create(articleId, request.AccountId, request.PermissionLevel);
            return StatusCode(201, articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}