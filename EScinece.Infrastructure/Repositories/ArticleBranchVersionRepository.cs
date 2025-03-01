using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class ArticleBranchVersionRepository(
    ILogger<ArticleBranchVersionRepository> logger,
    IDbConnectionFactory connectionFactory
    ) : IArticleBranchVersionRepository
{
    public async Task<ArticleBranchVersion?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<ArticleBranchVersion>(
                "SELECT * FROM article_branch_versions WHERE id = @id AND deleted_at IS NULL", new { id });
        });

    public async Task<IEnumerable<ArticleBranchVersion>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<ArticleBranchVersion>("SELECT * FROM article_branch_versions WHERE deleted_at IS NULL");
        });

    public async Task Create(ArticleBranchVersion entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(
                """
                INSERT INTO article_branch_versions (id, creator_id, article_branch, created_at, text)
                VALUES (@id, @creator_id, @article_branch, @created_at, @text)
                """, entity);
        });

    public async Task Update(ArticleBranchVersion entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(
                """
                UPDATE article_branch_versions
                SET text = @text,
                    updated_at = NOW()
                WHERE id = @id
                """, entity);
        });

    public async Task<bool> Delete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                """
                DELETE FROM article_branch_versions WHERE id = @id
                """, new { id });
            return result > 0;
        });

    public async Task<bool> SoftDelete(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                """
                UPDATE article_branch_versions
                SET deleted_at = NOW()
                WHERE id = @id
                """, new { id });
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