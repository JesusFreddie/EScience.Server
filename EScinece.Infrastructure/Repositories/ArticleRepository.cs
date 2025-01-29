using EScinece.Domain.Abstraction;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

public class ArticleRepository(EScienceDbContext context) : IArticleRepository
{
    private readonly EScienceDbContext _context = context;
    public Task<Article?> GetById(Guid id)
    {
        var article = _context
            .Article
            .AsNoTracking()
            .Include(a => a.ArticleBranches)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        return article;
    }

    public Task<ICollection<Article>> GetAllByArticleParticipantId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Article>> GetAllByArticleParticipantIdInCreator(Guid id)
    {
        throw new NotImplementedException();
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

    public async Task<Article> Create(Article entity)
    {
        await _context.Article.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Guid> Update(
        Guid id,
        string title, 
        string description, 
        TypeArticle? typeArticle, 
        bool isPrivate = false
    )
    {
        await _context.Article
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(c => c.Title, title)
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.TypeArticle, typeArticle)
                .SetProperty(c => c.IsPrivate, isPrivate)
                );
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Article
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return id;
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