using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Services;

public class ArticleParticipantService(IArticleParticipantRepository articleParticipantRepository) : IArticleParticipantService
{
    private readonly IArticleParticipantRepository _articleParticipantRepository = articleParticipantRepository;
    public Task<Result<ArticleParticipantDto, string>> Create(Guid accountId, Guid articleId, Guid? id)
    {
        // var articleParticipantService = ArticleParticipant.Create();
        throw new NotImplementedException();
    }

    public Task<ArticleParticipantDto?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}