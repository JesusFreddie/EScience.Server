using EScience.Application.Handlers;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EScience.Application.Policy;

public static class ArticlePolicy
{
    public const string ArticleReaderPolicy = nameof(ArticleReaderPolicy);
    public static AuthorizationOptions AddArticlePolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(ArticleReaderPolicy, 
            policy => policy.Requirements.Add(new ArticlePermissionRequirement()
            {
                RequiredPermissionLevel = ArticlePermissionLevel.READER
            }));
        return options;
    }
}