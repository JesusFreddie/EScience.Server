using System.Text.Json;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace EScinece.Infrastructure.Repositories;

public class ArticleParticipantRepository(
    IDbConnectionFactory connectionFactory, 
    IDistributedCache cache
    ) : IArticleParticipantRepository
{
    public async Task<IEnumerable<ArticleParticipant>> GetAll()
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<ArticleParticipant>("SELECT * FROM article_participants");
    }

    public async Task Create(ArticleParticipant articleParticipant)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            """
            INSERT INTO article_participants (id, account_id, is_accepted, article_id, "article_Id")
            VALUES (@Id, @AccountId, @IsAccepted, @ArticleId, @ArticleId)
            """, articleParticipant);
        
        await cache.SetStringAsync("article_participants:", JsonSerializer.Serialize(articleParticipant),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });
    }

    public Task<ArticleParticipant> Update(ArticleParticipant entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ArticleParticipant?> GetById(Guid id)
    {
        var articleParticipantCache = await cache.GetStringAsync("article_participants:" + id);
        
        if (articleParticipantCache is not null)
            return JsonSerializer.Deserialize<ArticleParticipant>(articleParticipantCache);
        
        using var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstAsync<ArticleParticipant>(
            "SELECT * FROM article_participants WHERE id = @id", new { id });
    }

    public async Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}