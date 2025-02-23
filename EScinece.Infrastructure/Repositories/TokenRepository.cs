using Dapper;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace EScinece.Infrastructure.Repositories;

public class TokenRepository
    (ILogger<TokenRepository> logger, IDbConnectionFactory connectionFactory): ITokenRepository
{
    public async Task<RefreshToken> CreateRefreshToken(RefreshToken refreshToken) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                """
                INSERT INTO refresh_tokens (account_id, token, expires)
                VALUES (@account_id, @token, @expires)
                """, refreshToken);
            return refreshToken;
        });

    public async Task<RefreshToken?> GetRefreshToken(Guid accountId) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<RefreshToken>(
                "SELECT * FROM refresh_tokens WHERE account_id = @accountId", new { accountId });
        });

    public async Task<Guid?> GetAccountIdByRefreshToken(string refreshToken) =>
        await ExecuteWithExceptionHandlingAsync(async () =>
        {
            var connection = await connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Guid>(
                """
                SELECT account_id
                FROM refresh_tokens
                WHERE token = @token
                """, new { token = refreshToken });
        });

    private Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return func();
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "Произошла ошибка SQL-запроса");
            throw new Exception("Ошибка SQL-запроса: " + ex.Message, ex);
        }
        catch (DbConnectionException ex)
        {
            logger.LogError(ex, "Ошибка соединения");
            throw new Exception("Ошибка соединения: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Неизвестная ошибка");
            throw new Exception("Неизвестная ошибка: " + ex.Message, ex);
        }
    }
}