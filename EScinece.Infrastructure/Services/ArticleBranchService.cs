using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleBranchService(
    IArticleBranchRepository articleBranchRepository,
    IArticleVersionService articleVersionService,
    IArticleParticipantService articleParticipantService,
    ILogger<ArticleBranchService> logger
    ) : IArticleBranchService
{
    public async Task<Result<ArticleBranch, string>> Create(string name, Guid accountId, Guid articleId, Guid? parentId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ArticleBranchErrorMessage.NameIsRequired;
        
        if (await GetByTitle(name, articleId) is not null)
            return "Branch already exists";

        var participant = await articleParticipantService.GetByAccount(accountId, articleId);
        
        if (participant is null)
            return "Participant not found";
        
        if (parentId is null)
        {
            var main = await GetMain(articleId);
            if (main is not null)
            {
                parentId = main.Id;
            }
        }
        
        var articleBranchResult = ArticleBranch.Create(name, articleId, participant.Id, parentId, parentId is null);
        if (!articleBranchResult.onSuccess)
            return articleBranchResult.Error;
        try
        {
            await articleBranchRepository.Create(articleBranchResult.Value);

            if (parentId is not null)
            {
                await articleVersionService.CopyByBranch(parentId.Value, participant.Id, articleBranchResult.Value.Id);
            }
            else
            {
                await articleVersionService.Create("Test text", participant.Id, articleBranchResult.Value.Id);
            }

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

    public async Task<IEnumerable<ArticleBranch>> GetAllByArticleId(Guid articleId)
    {
        try
        {
            return await articleBranchRepository.GetAll(articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Server error getting all article branches");
        }
    }

    public async Task<ArticleBranch?> GetMain(Guid articleId)
    {
        try
        {
            return await articleBranchRepository.GetMain(articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}