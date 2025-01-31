using System.Text.Json;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace EScinece.Infrastructure.Repositories;

public class ArticleRepository(IDbConnectionFactory connectionFactory, IDistributedCache cache) : IArticleRepository
{
    public async Task<Article?> GetById(Guid id)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<Article>(
            "SELECT * FROM articles WHERE id = @id", new { id });
    }

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantIdInCreator(Guid id)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Article>(
            "SELECT * FROM articles WHERE creator_id = @id", new { id });
    }

    public async Task<IEnumerable<Article>> GetAll()
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Article>("SELECT * FROM articles");
        
    }

    public async Task Create(Article entity)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            """
            INSERT INTO articles (id, title, description, is_private, creator_id, type_article_id)
            VALUES (@Id, @Title, @Description, @IsPrivate, @CreatorId, @TypeArticleId)
            """, entity);

        await cache.SetStringAsync("article:" + entity.Id, JsonSerializer.Serialize(entity), 
            new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        });
    }

    public async Task<Article> Update(Article entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Article>> GetByPage(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
}