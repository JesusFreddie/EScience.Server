using EScience.Application.Handlers;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EScience.Application.Policy;

public static class ArticlePolicy
{
    public const string ArticleReaderPolicy = nameof(ArticleReaderPolicy);
    public const string ArticleEditorBranchPolicy = nameof(ArticleEditorBranchPolicy);
    public const string ArticleEditorPolicy = nameof(ArticleEditorPolicy);
    public const string ArticleAuthorPolicy = nameof(ArticleAuthorPolicy);
    public static AuthorizationOptions AddArticlePolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(ArticleReaderPolicy, 
            policy => policy.Requirements.Add(new ArticlePermissionRequirement(ArticlePermissionLevel.READER)));
        options.AddPolicy(ArticleEditorBranchPolicy,
            policy => policy.Requirements.Add(new ArticlePermissionRequirement(ArticlePermissionLevel.EDITOR_BRANCH)));
        options.AddPolicy(ArticleEditorPolicy,
            policy => policy.Requirements.Add(new ArticlePermissionRequirement(ArticlePermissionLevel.EDITOR)));
        options.AddPolicy(ArticleAuthorPolicy,
            policy => policy.Requirements.Add(new ArticlePermissionRequirement(ArticlePermissionLevel.AUTHOR)));
        return options;
    }
}