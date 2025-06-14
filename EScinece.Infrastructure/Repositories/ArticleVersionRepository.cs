using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class ArticleVersionRepository(
    ILogger<ArticleVersionRepository> logger,
    IDbConnectionFactory connectionFactory
    ) : IArticleVersionRepository
{
    public async Task<ArticleVersion?> GetById(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<ArticleVersion>(
                "SELECT * FROM article_branch_versions WHERE id = @id AND deleted_at IS NULL", new { id });
        });

    public async Task<IEnumerable<ArticleVersion>> GetAll() =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<ArticleVersion>("SELECT * FROM article_branch_versions WHERE deleted_at IS NULL");
        });

    public async Task Create(ArticleVersion entity) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteAsync(
                """
                INSERT INTO article_branch_versions (id, creator_id, article_branch_id, text, updated_at)
                VALUES (@id, @creatorId, @articleBranchId, @text, NOW())
                """, entity);
        });

    public async Task Update(ArticleVersion entity) =>
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

    public async Task<bool> Exists(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(
                "SELECT EXISTS(SELECT 1 FROM article_branch_versions WHERE id = @id)", new { id });
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

    public async Task<ArticleVersion?> GetLatestVersion(Guid branchId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();

            return await connection.QueryFirstOrDefaultAsync<ArticleVersion>(
                """
                SELECT *
                FROM article_branch_versions
                WHERE article_branch_id = @branchId
                ORDER BY created_at DESC
                LIMIT 1
                """, new { branchId }
                );
        });

    public async Task<ArticleVersion?> GetFirstVersion(Guid branchId) => 
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();

            return await connection.QueryFirstOrDefaultAsync<ArticleVersion>(
                """
                SELECT *
                FROM article_branch_versions
                WHERE article_branch_id = @branchId
                ORDER BY created_at
                LIMIT 1
                """, new { branchId }
            );
        });

    public async Task<bool> SaveText(Guid versionId, string text) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                """
                UPDATE article_branch_versions
                SET text = @text,
                    updated_at = NOW()
                WHERE id = @versionId
                """, new { versionId, text }
                );
            return result > 0;
        });

    public async Task<IEnumerable<VersionInfo>> GetAllVersionInfo(Guid branchId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<VersionInfo>(
                """
                SELECT v.id, v.created_at, v.updated_at, v.article_branch_id, v.creator_id
                FROM article_branch_versions AS v
                WHERE article_branch_id = @branchId
                AND deleted_at IS NULL
                ORDER BY v.created_at DESC
                """, new { branchId });
        });

    public async Task<VersionInfo?> GetLastVersionInfoAsync(Guid branchId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<VersionInfo>(
                """
                SELECT v.id, v.created_at, v.updated_at, v.article_branch_id, v.creator_id
                FROM article_branch_versions AS v
                WHERE v.article_branch_id = @branchId
                ORDER BY v.created_at DESC
                LIMIT 1
                """, new { branchId });
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