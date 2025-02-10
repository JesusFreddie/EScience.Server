using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EScience.Application.Handlers;

public class ArticleAuthorizationHandler : AuthorizationHandler<ArticlePermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ArticleAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ArticlePermissionRequirement requirement)
    {
        var accountId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.AccountId);
        
        if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
        {
            return;
        }

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return;

        if (!httpContext.Request.RouteValues.TryGetValue("id", out var idValue)
            || !Guid.TryParse(idValue?.ToString(), out var articleId))
            return;
        
        using var scope = _serviceScopeFactory.CreateScope();
        var articleParticipantService = scope.ServiceProvider.GetRequiredService<IArticleParticipantService>();
        var permission = await articleParticipantService.GetArticlePermissionLevelByIds(id, articleId);

        if (permission >= requirement.RequiredPermissionLevel)
        {
            context.Succeed(requirement);
        }
    }
}

public class ArticlePermissionRequirement : IAuthorizationRequirement
{
    public ArticlePermissionLevel  RequiredPermissionLevel { get; set; }
}