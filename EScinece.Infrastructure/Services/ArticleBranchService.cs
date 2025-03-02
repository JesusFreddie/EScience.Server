using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleBranchService(
    IArticleBranchRepository articleBranchRepository,
    IArticleBranchVersionService articleBranchVersionService,
    ILogger<ArticleBranchService> logger
    ) : IArticleBranchService
{
    public async Task<Result<ArticleBranchDto, string>> Create(string name, Guid creatorId, Guid articleId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ArticleBranchErrorMessage.NameIsRequired;
        
        var articleBranchResult = ArticleBranch.Create(name, creatorId, articleId);
        if (!articleBranchResult.onSuccess)
            return articleBranchResult.Error;

        try
        {
            var articleBranch = articleBranchResult.Value;
            
            await articleBranchRepository.Create(articleBranch);
            await articleBranchVersionService.Create("", creatorId, articleBranch.Id);

            return new ArticleBranchDto(
                Id: articleBranch.Id,
                Name: articleBranch.Name,
                ArticleId: articleBranch.ArticleId,
                CreatorId: creatorId,
                CreatedAt: articleBranch.CreatedAt,
                UpdatedAt: articleBranch.UpdatedAt);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Server error creating article branch");
        }
    }

    public async Task<ArticleBranchDto?> GetById(Guid id)
    {
        var articleBranch = await articleBranchRepository.GetById(id);
        if (articleBranch is null)
            return null;
        
        return new ArticleBranchDto(
            Id: articleBranch.Id,
            Name: articleBranch.Name,
            ArticleId: articleBranch.ArticleId,
            CreatorId: articleBranch.CreatorId,
            CreatedAt: articleBranch.CreatedAt,
            UpdatedAt: articleBranch.UpdatedAt);
    }
}