using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using EScience.Application.Handlers;
namespace EScience.Application.Configuration;

public static class AuthorizationPolicyConfiguration
{
    public const string ARTICLE_READER_POLICY = nameof(ARTICLE_READER_POLICY);
    public static AuthorizationOptions AddArticlePolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(ARTICLE_READER_POLICY, 
            policy => policy.Requirements.Add(new ArticlePermissionRequirement()
            {
                RequiredPermissionLevel = ArticlePermissionLevel.READER
            }));
        return options;
    }
}