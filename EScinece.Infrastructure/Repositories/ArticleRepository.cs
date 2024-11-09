using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

internal class ArticleRepository : IRepository<Article>, IPaginations<Article>
{
    private readonly EScienceDbContext _context;
    
    public ArticleRepository(EScienceDbContext context)
    {
        _context = context;
    }

    public Task<Article?> GetById(Guid id)
    {
        var article = _context
            .Article
            .AsNoTracking()
            .Include(a => a.ArticleBranches)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        return article;
    }

    public async Task<List<Article>> GetAll()
    {
        var articles = await _context
            .Article
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync();
        
        return articles;
    }

    public async Task Create(Article entity)
    {
        var article = new Article
        {
            CreatedOn = entity.CreatedOn,
            ArticleBranches = entity.ArticleBranches,
            Creator = entity.Creator,
            Description = entity.Description,
            Title = entity.Title,
            ArticleParticipants = entity.ArticleParticipants,
            CreatorId = entity.CreatorId
        };
        
        await _context.Article.AddAsync(article);
        await _context.SaveChangesAsync();
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
        return await _context.Article
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}