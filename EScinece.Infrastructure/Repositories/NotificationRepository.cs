using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class NotificationRepository(
    ILogger<ArticleVersionRepository> logger,
    IDbConnectionFactory connectionFactory
) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid accountId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Notification>(
                "SELECT * FROM notification WHERE account_id = @accountId ORDER BY id DESC", new { accountId });
        });

    public async Task<Notification> Create(Notification notification) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO notification (account_id, title, message, type)
                VALUES (@accountId, @title, @message, @type)
                """, notification);
            return notification;
        });

    public async Task MarkAsReadAsync(int notificationId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                UPDATE notification
                SET is_read = true,
                    readed_date = NOW()
                WHERE id = @notificationId
                """, new { notificationId });
            return Task.CompletedTask;
        });

    public async Task MarkAsReadAllAsync(Guid accountId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                UPDATE notification
                SET is_read = true,
                    readed_date = NOW()
                WHERE account_id = @accountId
                """, new { accountId });
            return Task.CompletedTask;
        });

    public async Task<int> GetCountNotificationsAsync(Guid accountId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT count(*) FROM notification WHERE account_id = @accountId AND is_read = false", new { accountId });
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