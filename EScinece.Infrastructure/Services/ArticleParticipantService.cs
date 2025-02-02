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
    public async Task<Result<ArticleParticipantDto, string>> Create(Guid accountId, Guid articleId, Guid? id)
    {
        try
        {
            var articleParticipantResult = ArticleParticipant.Create(accountId, articleId);
            
            if (!articleParticipantResult.onSuccess)
                return articleParticipantResult.Error;

            var articleParticipant = articleParticipantResult.Value;
            await articleParticipantRepository.Create(articleParticipant);

            return new ArticleParticipantDto(
                Id: articleParticipant.Id,
                AccountId: articleParticipant.AccountId,
                IsAccepted: articleParticipant.IsAccepted,
                ArticleId: articleParticipant.ArticleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла серверная ошибка при создании участника стать");
            throw new Exception("Произошла серверная ошибка при создании участника стать");
        }
    }

    public async Task<ArticleParticipantDto?> GetById(Guid id)
    {
        try
        {
            var articleParticipant = await articleParticipantRepository.GetById(id);
            if (articleParticipant is null)
                return null;
            
            return new ArticleParticipantDto(
                Id: articleParticipant.Id,
                AccountId: articleParticipant.AccountId,
                IsAccepted: articleParticipant.IsAccepted,
                ArticleId: articleParticipant.ArticleId);
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
}