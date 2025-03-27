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
    public async Task<Result<ArticleBranch, string>> Create(string name, Guid creatorId, Guid articleId)
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

            return articleBranchResult.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Server error creating article branch");
        }
    }

    public async Task<ArticleBranch?> GetById(Guid id)
    {
        try
        {
            return await articleBranchRepository.GetById(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<ArticleBranch?> GetByTitle(string name, Guid articleId)
    {
        try
        {
            return await articleBranchRepository.GetByTitle(name, articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}