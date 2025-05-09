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
    IArticleBranchService articleBranchService,
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
                    permissionLevel: ArticlePermissionLevel.AUTHOR,
                    isAccepted: true
                    );
                
                if (!creator.onSuccess)
                {
                    await articleRepository.Delete(article.Value.Id);
                    return creator.Error;
                }

                try
                {
                    await articleBranchService.Create("main", accountId, article.Value.Id);

                    return article;
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                    await articleParticipantService.Delete(creator.Value.Id);
                    throw;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
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

    public async Task<Result<Article, string>> Update(Guid id, string? title, string? description, bool? isPrivate)
    {
        try
        {
            var article = await GetById(id);
            if (article is null)
                return ArticleErrorMessage.ArticleNotFound;
            
            article.Title = title ?? article.Title;
            article.Description = description ?? article.Description;
            article.IsPrivate = isPrivate ?? article.IsPrivate;
            
            await articleRepository.Update(article);
            
            return article;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception("Error update article");
        }
    }

    public Task<int> GetCount(Guid accountId)
    {
        try
        {
            return articleRepository.GetCount(accountId);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception("Error get count article");
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

    public async Task<IEnumerable<Article>> GetAllByAccountId(Guid id, int take)
    {
        const int limit = 8;
        var skip = limit * (take - 1);
        try
        {
            return await articleRepository.GetAllByAccountId(id, limit, skip);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception("Error get all articles");
        }
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

    public async Task<Article?> GetByTitle(string title, Guid accountId, string? branchName = null)
    {
        try
        {
            return await articleRepository.GetByTitle(title, accountId, branchName);
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