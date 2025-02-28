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

public class ArticleRepository(
    IDbConnectionFactory connectionFactory, 
    IDistributedCache cache,
    ILogger<ArticleRepository> logger
    ) : IArticleRepository
{
    public async Task<Article?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Article>(
                "SELECT * FROM articles WHERE id = @id AND deleted_at IS NULL", new { id });
        });

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantIdInCreator(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Article>(
                """
                    SELECT articles.*
                    FROM articles
                    JOIN public.article_participants ap on articles.id = ap.article_id
                    WHERE ap.account_id = @id
                    AND ap.permission_level = @permission_level
                    AND articles.deleted_at IS NULL
                    """, new { id, permission_level = ArticlePermissionLevel.AUTHOR });
        });

    public async Task<IEnumerable<Article>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Article>("SELECT * FROM articles WHERE deleted_at IS NULL");
        });

    public async Task Create(Article entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO articles (id, title, description)
                VALUES (@id, @title, @description)
                """, entity);

            await cache.SetStringAsync("article:" + entity.Id, JsonSerializer.Serialize(entity),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });

            return Task.CompletedTask;
        });

    public async Task Update(Article entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                UPDATE articles
                SET title = @title, 
                    description = @description, 
                    is_private = @is_private, 
                    updated_at = NOW()
                WHERE id = @id
                """, entity);
            
            return Task.CompletedTask;
        });

    public async Task<bool> Delete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                "DELETE FROM articles WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<bool> SoftDelete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                "UPDATE articles SET deleted_at = NOW() WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<List<Article>> GetByPage(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

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