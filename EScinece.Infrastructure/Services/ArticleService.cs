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
    IAccountService accountService,
    ILogger<ArticleService> logger
    ) : IArticleService
{
    
    public async Task<Result<Article, string>> Create(
        string title, 
        string description, 
        Guid accountId,
        bool isPrivate,
        Guid? typeArticleId)
    {
        try
        {
            var validate = IsValidate(title, description);

            if (!string.IsNullOrEmpty(validate))
                return validate;

            var articleExist = await GetByTitle(title, accountId);
            if (articleExist is not null)
                return ArticleErrorMessage.ArticleTitleExists;
            
            var article = Article.Create(
                title: title,
                description: description,
                accountId: accountId,
                isPrivate: isPrivate,
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

                return article;
            }
            catch
            {
                await articleRepository.Delete(article.Value.Id);
                throw;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception("Произошла серверная ошибка при создании статьи");
        }
    }

    public Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantIdAndAccountId(Guid id)
    {
        return await articleRepository.GetAllByArticleParticipantIdAndAccountId(id);
    }

    public Task<IEnumerable<Article>> GetAllByAccountId(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Article?> GetById(Guid id)
    {
        try
        {
            return await articleRepository.GetById(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Ошибка при получении статьи");
        }
    }

    public async Task<Article?> GetByTitle(string title, Guid accountId)
    {
        try
        {
            return await articleRepository.GetByTitle(title, accountId);
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