using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using EScience.Application.Handlers;
namespace EScience.Application.Configuration;

public static class AuthorizationPolicyConfiguration
{
    public static AuthorizationOptions AddArticlePolicies(this AuthorizationOptions options)
    {
        options.AddPolicy("ArticleReaderPolicy", 
            policy => policy.Requirements.Add(new ArticlePermissionRequirement()
            {
                RequiredPermissionLevel = ArticlePermissionLevel.READER
            }));
        return options;
    }
}