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
    
    public const string BranchReaderPolicy = nameof(BranchReaderPolicy);
    public const string BranchEditorBranchPolicy = nameof(BranchEditorBranchPolicy);
    public const string BranchEditorPolicy = nameof(BranchEditorPolicy);
    public const string BranchAuthorPolicy = nameof(BranchAuthorPolicy);
    
    public const string VersionReaderPolicy = nameof(VersionReaderPolicy);
    public const string VersionEditorBranchPolicy = nameof(VersionEditorBranchPolicy);
    public const string VersionEditorPolicy = nameof(VersionEditorPolicy);
    public const string VersionAuthorPolicy = nameof(VersionAuthorPolicy);
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
        
        options.AddPolicy(BranchReaderPolicy, 
            policy => policy.Requirements.Add(new BranchPermissionRequirement(ArticlePermissionLevel.READER)));
        options.AddPolicy(BranchEditorBranchPolicy,
            policy => policy.Requirements.Add(new BranchPermissionRequirement(ArticlePermissionLevel.EDITOR_BRANCH)));
        options.AddPolicy(BranchEditorPolicy,
            policy => policy.Requirements.Add(new BranchPermissionRequirement(ArticlePermissionLevel.EDITOR)));
        options.AddPolicy(BranchAuthorPolicy,
            policy => policy.Requirements.Add(new BranchPermissionRequirement(ArticlePermissionLevel.AUTHOR)));
        
        options.AddPolicy(VersionReaderPolicy, 
            policy => policy.Requirements.Add(new VersionPermissionRequirement(ArticlePermissionLevel.READER)));
        options.AddPolicy(VersionEditorBranchPolicy,
            policy => policy.Requirements.Add(new VersionPermissionRequirement(ArticlePermissionLevel.EDITOR_BRANCH)));
        options.AddPolicy(VersionEditorPolicy,
            policy => policy.Requirements.Add(new VersionPermissionRequirement(ArticlePermissionLevel.EDITOR)));
        options.AddPolicy(VersionAuthorPolicy,
            policy => policy.Requirements.Add(new VersionPermissionRequirement(ArticlePermissionLevel.AUTHOR)));
        return options;
    }
}