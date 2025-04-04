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
    IArticleVersionService articleVersionService,
    ILogger<ArticleBranchService> logger
    ) : IArticleBranchService
{
    public async Task<Result<ArticleBranch, string>> Create(string name, Guid creatorId, Guid articleId)
    {
        Console.WriteLine("!@#");
        Console.WriteLine(articleId);
        if (string.IsNullOrWhiteSpace(name))
            return ArticleBranchErrorMessage.NameIsRequired;
        
        var articleBranchResult = ArticleBranch.Create(name, articleId, creatorId);
        if (!articleBranchResult.onSuccess)
            return articleBranchResult.Error;
        Console.WriteLine(articleBranchResult.Value.Id);
        try
        {
            // var articleBranch = articleBranchResult.Value;
            Console.WriteLine(articleBranchResult.Value.Id);
            await articleBranchRepository.Create(articleBranchResult.Value);
            await articleVersionService.Create("Test text", creatorId, articleBranchResult.Value.Id);

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