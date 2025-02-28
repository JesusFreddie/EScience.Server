using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class ArticleService(
    IArticleRepository articleRepository,
    IArticleParticipantService articleParticipantService,
    ILogger<ArticleService> logger
    ) : IArticleService
{
    
    public async Task<Result<ArticleDto, string>> Create(
        string title, 
        string description, 
        Guid accountId, 
        Guid? typeArticleId)
    {
        try
        {
            var validate = IsValidate(title, description);

            if (!string.IsNullOrEmpty(validate))
                return validate;

            var article = Article.Create(
                title: title,
                description: description,
                typeArticleId: typeArticleId);

            if (!article.onSuccess)
            {
                return article.Error;
            }

            await articleRepository.Create(article.Value);

            try
            {
                var creator = await articleParticipantService.Create(
                    accountId: accountId,
                    articleId: article.Value.Id,
                    permissionLevel: ArticlePermissionLevel.AUTHOR);

                if (!creator.onSuccess)
                {
                    await articleRepository.Delete(article.Value.Id);
                    return creator.Error;
                }
            }
            catch
            {
                await articleRepository.Delete(article.Value.Id);
                throw;
            }
            
            
            
            return new ArticleDto(
                Id: article.Value.Id,
                Title: article.Value.Title,
                Description: article.Value.Description,
                TypeArticleId: typeArticleId,
                IsPrivate: article.Value.IsPrivate
            );
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception("Произошла серверная ошибка при создании статьи");
        }
    }

    public Task<ICollection<ArticleDto>> GetAllByArticleParticipantId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ArticleDto>> GetAllByArticleParticipantIdInCreator(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ArticleDto?> GetById(Guid id)
    {
        try
        {
            var article = await articleRepository.GetById(id);

            if (article is null)
                return null;
            
            return new ArticleDto(
                Id: article.Id,
                Title: article.Title,
                Description: article.Description,
                TypeArticleId: article.TypeArticleId,
                IsPrivate: article.IsPrivate);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Ошибка при получении статьи");
        }
    }

    private static string IsValidate(string title, string description)
    {
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            return ArticleErrorMessage.TitleAndDescriptionCannotBeEmpty;
        
        return "";
    }
}