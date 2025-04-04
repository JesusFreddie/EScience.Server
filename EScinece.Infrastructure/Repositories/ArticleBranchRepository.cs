using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class ArticleBranchRepository(
    ILogger<ArticleBranchRepository> logger,
    IDbConnectionFactory connectionFactory
    ) : IArticleBranchRepository
{
    public async Task<ArticleBranch?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<ArticleBranch>(
                "SELECT * FROM article_branches WHERE id = @id AND deleted_at is not null", new { id });
        });

    public async Task<IEnumerable<ArticleBranch>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<ArticleBranch>(
                "SELECT * FROM article_branches WHERE deleted_at is not null");
        });

    public async Task Create(ArticleBranch entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(
                """
                INSERT INTO article_branches (id, name, creator_id, article_id)
                VALUES (@id, @name, @creatorId, @articleId)
                """, entity);
        });

    public async Task Update(ArticleBranch entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(
                """
                UPDATE article_branches
                SET name = @name,
                    updated_at = NOW()
                WHERE id = @id
                AND deleted_at is not null
                """, entity);
        });

    public async Task<bool> Delete(Guid id)=>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result =  await connection.ExecuteAsync(
                "DELETE FROM article_branches WHERE id = @id", new { id });
            return result > 0;
        });

    public async Task<bool> SoftDelete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result =  await connection.ExecuteAsync(
                """
                UPDATE article_branches
                SET deleted_at = NOW()
                WHERE id = @id
                AND deleted_at is not null
                """, new { id });
            return result > 0;
        });

    public async Task<ArticleBranch?> GetByTitle(string name, Guid articleId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.QueryFirstAsync<ArticleBranch>(
                """
                SELECT *
                FROM article_branches
                WHERE name = @name
                AND article_id = @articleId
                """, new { name, articleId }
                );
            return result;
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