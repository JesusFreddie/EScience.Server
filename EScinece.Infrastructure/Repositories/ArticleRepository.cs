using Dapper;
using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;

namespace EScinece.Infrastructure.Repositories;

public class ArticleRepository(EScienceDbContext context) : IRepository<Article>, IPaginations<Article>
{
    private readonly EScienceDbContext _context = context;
    public Task<Article?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Article>> GetAll()
    {
        using var dbConnection = await _context.CreateConnectionAsync();
        var result = await dbConnection.QueryAsync<Article>("SELECT * FROM articles");
        return result.ToList();
    }

    public async Task Create(Article entity)
    {
        throw new NotImplementedException();
    }

    public Task<Article> Update(Article entity)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> Delete(Article entity)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Article>> GetByPage(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
}