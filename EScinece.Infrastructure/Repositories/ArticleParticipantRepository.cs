using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EScinece.Infrastructure.Repositories;

public class ArticleParticipantRepository(EScienceDbContext context) : IArticleParticipantRepository
{
    private readonly EScienceDbContext _context = context;
    
    public async Task<ArticleParticipant> Create(ArticleParticipant articleParticipant)
    {
        await _context.ArticleParticipant.AddAsync(articleParticipant);
        await _context.SaveChangesAsync();
        return articleParticipant;
    }

    public async Task<ArticleParticipant?> GetById(Guid id)
    {
        return await _context.ArticleParticipant.FindAsync(id);
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.ArticleParticipant
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }
}