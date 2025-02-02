using System.Text.Json;
using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class ArticleParticipantRepository(
    IDbConnectionFactory connectionFactory, 
    IDistributedCache cache,
    ILogger<ArticleParticipantRepository> logger
    ) : IArticleParticipantRepository
{
    public async Task<IEnumerable<ArticleParticipant>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<ArticleParticipant>("SELECT * FROM article_participants WHERE deleted_at IS NULL");
        });

    public async Task Create(ArticleParticipant articleParticipant) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
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
            return Task.CompletedTask;
        });

    public async Task<ArticleParticipant?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var articleParticipantCache = await cache.GetStringAsync("article_participants:" + id);

            if (articleParticipantCache is not null)
                return JsonSerializer.Deserialize<ArticleParticipant>(articleParticipantCache);

            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<ArticleParticipant>(
                "SELECT * FROM article_participants WHERE id = @id AND deleted_at IS NULL", new { id });
        });

    public async Task<bool> Delete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                "DELETE FROM article_participants WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<bool> SoftDelete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                "UPDATE article_participants SET deleted_at = NOW() WHERE id = @id", new { id });
            return result > 0;
        });

    private async Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (NpgsqlException ex)
        {
            logger.LogError("Произошла ошибка SQL-запроса", ex);
            throw new Exception("Ошибка SQL-запроса: " + ex.Message, ex);
        }
        catch (RedisConnectionException ex)
        {
            logger.LogError("Ошибка соединения с Redis", ex);
            throw new Exception("Ошибка соединения с Redis: " + ex.Message, ex);
        }
        catch (DbConnectionException ex)
        {
            logger.LogError("Ошибка соединения", ex);
            throw new Exception("Ошибка соединения: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("Неизвестная ошибка", ex);
            throw new Exception("Неизвестная ошибка: " + ex.Message, ex);
        }
    }
}