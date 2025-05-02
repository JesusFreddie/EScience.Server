using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface IArticleService
{
    public Task<Result<Article, string>> Create(
        string title, 
        string description, 
        Guid accountId, 
        bool isPrivate,
        Guid? typeArticleId);

    public Task<Result<Article, string>> Update(
        Guid id,
        string? title,
        string? description,
        bool? isPrivate
        );
    public Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id);
    public Task<IEnumerable<Article>> GetAllByArticleParticipantIdAndAccountId(Guid id);
    public Task<IEnumerable<Article>> GetAllByAccountId(Guid id);
    public Task<Article?> GetById(Guid id);
    public Task<Article?> GetByTitle(string title, Guid accountId, string? branchName = null);
}