using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using StackExchange.Redis;

namespace EScinece.Infrastructure.Repositories;

public class AccountInfoRepository(
    ILogger<AccountInfoRepository> logger,
    IDbConnectionFactory connectionFactory
    ) : IAccountInfoRepository
{
    public async Task<AccountInfo?> GetAsync(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<AccountInfo>(
                """
                SELECT *
                FROM account_infos
                WHERE id = @id
                """, new { id });
        });

    public async Task<List<AccountInfo>> GetAllByAccountAsync(Guid accountId) =>
        (List<AccountInfo>)await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<AccountInfo>(
                """
                SELECT *
                FROM account_infos
                WHERE account_id = @accountId
                ORDER BY created_at
                """, new { accountId });
        });

    public async Task<bool> SaveAsync(AccountInfo accountInfo) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                """
                INSERT INTO account_infos (id, account_id, field, value)
                SELECT @id, @accountId, @field, @value
                WHERE NOT EXISTS (
                    SELECT 1 FROM account_infos 
                    WHERE account_id = @accountId AND field = @field
                );
                """, accountInfo);
            return result != 0;
        });
    
    public async Task<bool> DeleteAsync(Guid id) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                """
                DELETE FROM account_infos
                WHERE id = @id
                """, new { id });
            return result != 0;
        });

    public async Task<bool> UpdateAsync(AccountInfo accountInfo) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            
            var result = await connection.ExecuteAsync(
                """
                UPDATE account_infos
                SET field = @field, value = @value, updated_at = NOW()
                WHERE id = @id
                AND account_id = @accountId
                AND NOT EXISTS (
                    SELECT 1 FROM account_infos
                    WHERE account_id = @accountId
                    AND field = @field
                    AND id != @id
                );
                """, accountInfo);
            return result != 0;
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