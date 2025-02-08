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
    
    public async Task<Result<ArticleDto, string>> Create(string title, string description, Guid accountId, Guid typeArticleId)
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

            var creator = await articleParticipantService.Create(accountId, typeArticleId);

            if (!creator.onSuccess)
            {
                await articleRepository.Delete(article.Value.Id);
                return creator.Error;
            }

            return new ArticleDto(
                Id: article.Value.Id,
                Title: article.Value.Title,
                Description: article.Value.Description,
                TypeArticleId: typeArticleId
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

    public Task<ArticleDto?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    private static string IsValidate(string title, string description)
    {
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            return ArticleErrorMessage.TitleAndDescriptionCannotBeEmpty;
        
        return "";
    }
}