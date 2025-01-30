using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Services;

public class ArticleService(
    IArticleRepository articleRepository,
    IArticleParticipantService articleParticipantService
    ) : IArticleService
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IArticleParticipantService _articleParticipantService = articleParticipantService;
    
    public async Task<Result<ArticleDto, string>> Create(string title, string description, Guid accountId, Guid typeArticleId)
    {
        var validate = IsValidate(title, description);

        if (!string.IsNullOrEmpty(validate))
            return validate;
        
        var creatorId = Guid.NewGuid();
        var articleId = Guid.NewGuid();
        
        var creator = await _articleParticipantService.Create(accountId, typeArticleId, creatorId);
        
        if (!creator.onSuccess)
            return creator.Error;

        var article = Article.Create(
            title: title,
            description: description,
            creatorId: creator.Value.Id,
            typeArticleId: typeArticleId,
            id: articleId);

        if (!article.onSuccess)
        {
            await _articleParticipantService.Delete(creator.Value.Id);
            return article.Error;
        }
        
        await _articleRepository.Create(article.Value);

        return new ArticleDto(
            Id: article.Value.Id,
            Title: article.Value.Title,
            Description: article.Value.Description,
            CreatorId: creator.Value.Id,
            TypeArticleId: typeArticleId
            );
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