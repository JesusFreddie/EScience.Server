using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface IArticleBranchRepository : IRepository<ArticleBranch>
{
    public Task<ArticleBranch?> GetByTitle(string name, Guid articleId);
    public Task<ArticleBranch?> GetMain(Guid articleId);
    public Task<IEnumerable<ArticleBranch>> GetAll(Guid articleId);
}