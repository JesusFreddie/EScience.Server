using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EScience.Application.Handlers;

public class VersionPolicyHandler : AuthorizationHandler<VersionPermissionRequirement>
{
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public VersionPolicyHandler(
        IServiceScopeFactory serviceScopeFactory, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<ArticlePolicyHandler> logger
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, VersionPermissionRequirement requirement)
    {
        var accountId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.AccountId);
        
        if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
        {
            return;
        }
        
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return;
        
        if (!httpContext.Request.RouteValues.TryGetValue("versionId", out var idValue)
            || !Guid.TryParse(idValue?.ToString(), out var versionId))
            return;
        
        using var scope = _serviceScopeFactory.CreateScope();
        
        var versionService = scope.ServiceProvider.GetRequiredService<IArticleVersionService>();
        var version = await versionService.GetById(versionId);
        
        if (version is null)
            return;
        
        if (version.CreatorId == id)
            context.Succeed(requirement);
        
        var branchService = scope.ServiceProvider.GetRequiredService<IArticleBranchService>();
        
        var branch = await branchService.GetById(version.ArticleBranchId);

        if (branch is null)
            return;
        
        var articleParticipantService = scope.ServiceProvider.GetRequiredService<IArticleParticipantService>();
        var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();

        var article = await articleService.GetById(branch.ArticleId);

        if (article is null)
            return;
        
        if (!article.IsPrivate && requirement.RequiredPermissionLevel == ArticlePermissionLevel.READER)
            context.Succeed(requirement);
        
        var permission = await articleParticipantService.GetArticlePermissionLevelByIds(id, branch.ArticleId);
        
        if (permission >= requirement.RequiredPermissionLevel)
        {
            context.Succeed(requirement);
        }
    }
}

public class VersionPermissionRequirement(ArticlePermissionLevel permissionLevel) : IAuthorizationRequirement
{
    public ArticlePermissionLevel RequiredPermissionLevel { get; set; } = permissionLevel;
}