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
    ILogger<ArticleController> logger) : ControllerBase
{
    
    [HttpPost("create")]
    public async Task<ActionResult<ArticleDto>> Create([FromBody] CreateArticleRequest request)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.UserId)!;
            
            Console.WriteLine(userId.Value);
            
            Guid.TryParse(userId.Value, out var id);
            
            var article = await articleService.Create(
                title: request.Title,
                description: request.Description,
                accountId: id,
                typeArticleId: request.ArticleTypeId);
            
            if (!article.onSuccess)
            {
                return BadRequest(article.Error);
            }

            return article.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}