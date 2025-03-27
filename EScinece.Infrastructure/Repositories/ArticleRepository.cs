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
            const string sql = 
                """
                    SELECT a.*, ac.*
                    FROM articles as a
                    INNER JOIN accounts as ac on a.account_id = ac.id
                    WHERE a.id = @id
                    AND a.deleted_at IS NULL
                """;
            var result = await connection.QueryAsync<Article, Account, Article>(
                sql,
                (article, account) => 
                {
                    article.Account = account;
                    return article;
                },
                new { id },
                splitOn: "id"
            );
            return result.FirstOrDefault();
        });

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantId(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Article, Account, Article>(
                """
                SELECT articles.*,  a.*
                FROM articles
                JOIN public.article_participants ap on articles.id = ap.article_id
                JOIN accounts as a on articles.account_id = a.id
                WHERE ap.account_id = @id
                AND articles.deleted_at IS NULL
                """, (article, account) => 
                {
                    article.Account = account;
                    return article;
                },
                new { id });
        });

    public async Task<IEnumerable<Article>> GetAllByAccountId(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Article, Account, Article>(
                """
                    SELECT a.*, ac.*
                    FROM articles as a
                    INNER JOIN accounts as ac ON a.account_id = ac.id
                    WHERE account_id = @id
                    AND a.deleted_at IS NULL
                    """, 
                (article, account) => 
                {
                    article.Account = account;
                    return article;
                },
                new { id },
                splitOn: "id");
        });

    public async Task<IEnumerable<Article>> GetAllByArticleParticipantIdAndAccountId(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Article, Account, Article>(
                """
                SELECT articles.*, a.*
                FROM articles
                JOIN public.article_participants ap on articles.id = ap.article_id
                INNER JOIN accounts as a on articles.account_id = a.id
                WHERE (ap.account_id = @id
                OR articles.account_id = @id)
                AND articles.deleted_at IS NULL
                """, (article, account) => 
                {
                    article.Account = account;
                    return article;
                },
                new { id },
                splitOn: "id");
        });
    public async Task<Article?> GetByTitle(string title, Guid accountId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            const string sql = """
                SELECT a.*, acc.* 
                FROM articles a
                INNER JOIN accounts acc ON a.account_id = acc.id
                WHERE a.title = @title
                AND a.account_id = @accountId
                LIMIT 1
                """;

            var result = await connection.QueryAsync<Article, Account, Article>(
                sql,
                (article, account) => 
                {
                    article.Account = account;
                    return article;
                },
                new { title, accountId },
                splitOn: "id"
            );
            return result.FirstOrDefault();
        });

    public async Task<IEnumerable<Article>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            
            const string sql = 
                """
                SELECT a.*, acc.* 
                FROM articles a
                INNER JOIN accounts acc ON a.account_id = acc.id
                WHERE a.deleted_at IS NULL
                """;

            return await connection.QueryAsync<Article, Account, Article>(
                sql,
                (article, account) => 
                {
                    article.Account = account;
                    return article;
                },
                splitOn: "id"
            );
        });

    public async Task Create(Article entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO articles (id, title, description, account_id)
                VALUES (@id, @title, @description, @accountId)
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
            using var connection = await connectionFactory.CreateConnectionAsync();
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