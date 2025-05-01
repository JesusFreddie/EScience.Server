using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleParticipantService(
    IArticleParticipantRepository articleParticipantRepository,
    ILogger<ArticleParticipantRepository> logger) : IArticleParticipantService
{
    public async Task<Result<ArticleParticipant, string>> Create(
        Guid accountId, 
        Guid articleId, 
        ArticlePermissionLevel permissionLevel = ArticlePermissionLevel.READER)
    {
        try
        {
            var articleParticipantResult = ArticleParticipant.Create(accountId, articleId, permissionLevel);
            
            if (!articleParticipantResult.onSuccess)
                return articleParticipantResult.Error;

            var articleParticipant = articleParticipantResult.Value;
            await articleParticipantRepository.Create(articleParticipant);
            return articleParticipantResult.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка при создании участника стать");
            throw new Exception("Произошла серверная ошибка при создании участника стать");
        }
    }

    public async Task<ArticleParticipant?> GetById(Guid id)
    {
        try
        {
            return await articleParticipantRepository.GetById(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка получения статьи");
            throw new Exception("Произошла ошибка получения статьи");
        }
    }

    public Task<bool> Delete(Guid id)
    {
        try
        {
            return articleParticipantRepository.Delete(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка удаления статьи");
            throw new Exception("Произошла ошибка удаления статьи");
        }
    }

    public async Task<ArticleParticipant?> GetByAccount(Guid accountId, Guid articleId)
    {
        try
        {
            return await articleParticipantRepository.GetByAccount(accountId, articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Ошибка получения персисий");
        }
    }

    public Task<ArticlePermissionLevel> GetArticlePermissionLevelByIds(Guid accountId, Guid articleId)
    {
        try
        {
            return articleParticipantRepository.GetArticlePermissionLevelByIds(accountId, articleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Ошибка получения пермисий");
        }
    }
}