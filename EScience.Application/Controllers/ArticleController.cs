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
[Authorize]
[Route("articles")]
public class ArticleController(
    IArticleService articleService,
    IArticleParticipantService articleParticipantService,
    ILogger<ArticleController> logger
    ) : ControllerBase
{
    [HttpPost("create", Name = "ArticleCreate")]
    public async Task<ActionResult<Article>> Create(
        [FromBody] CreateArticleRequest request
        )
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
                isPrivate: request.IsPrivate,
                typeArticleId: request.ArticleTypeId);
         
            if (!article.onSuccess)
                return BadRequest(article.Error);
            
            return StatusCode(201, article.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet(Name = "ArticleGetAll")]
    public async Task<ActionResult<IEnumerable<Article>>> GetAll()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();

            var articles = await articleService.GetAllByArticleParticipantIdAndAccountId(id);
            
            return Ok(articles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("{articleId}", Name = "ArticleGet")]
    [Authorize(Policy = ArticlePolicy.ArticleReaderPolicy)]
    public async Task<ActionResult<Article?>> GetById(Guid articleId)
    {
        try
        {
            var article = await articleService.GetById(articleId);
            if (article is null)
                return NotFound();
            return Ok(article);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("set_participant_for_article/{articleId}", Name = "ArticleSetParticipant")]
    [Authorize(Policy = ArticlePolicy.ArticleAuthorPolicy)]
    public async Task<ActionResult<ArticleParticipant>> SetParticipantForArticle(Guid articleId, [FromBody] SetParticipantRequest request)
    {
        try
        {
            var result = await articleParticipantService.Create(articleId, request.AccountId, request.PermissionLevel);
            if (!result.onSuccess)
                return BadRequest(result.Error);
            return StatusCode(201, "Article participant added");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}