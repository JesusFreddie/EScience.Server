using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleRepository : IPaginations<Article>
{
    public Task<Article> Create(Article article);
    public Task<Article?> GetById(Guid id);
    public Task<ICollection<Article>> GetAllByArticleParticipantId(Guid id);
    public Task<ICollection<Article>> GetAllByArticleParticipantIdInCreator(Guid id);
    public Task<Guid> Update(
        Guid id,
        string title, 
        string description, 
        TypeArticle? typeArticle, 
        bool isPrivate = false
        );
    public Task<Guid> Delete(Guid id);
}