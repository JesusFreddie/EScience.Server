using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EScience.Application.Handlers;

public class ArticleAuthorizationHandler : AuthorizationHandler<ArticlePermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public ArticleAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ArticlePermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }
        
        using var scope = _serviceScopeFactory.CreateScope();
        var articleParticipantService = scope.ServiceProvider.GetRequiredService<IArticleParticipantService>();
        var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        
        var account = await accountService.GetByUserId(id);
        if (account is null)
            return;

        var permission = await articleParticipantService.GetArticlePermissionLevelByAccountId(account.Id);

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