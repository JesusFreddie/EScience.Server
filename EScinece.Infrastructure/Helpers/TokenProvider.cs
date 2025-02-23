using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Security;

namespace EScinece.Infrastructure.Helpers;
public class TokenProvider(
    IOptions<JwtOptions> options,
    TokenRepository tokenRepository,
    ILogger<TokenProvider> logger) : ITokenProvider
{
    private readonly JwtOptions _options = options.Value;
    public async Task<TokensDto> GenerateToken(Guid accountId)
    {
        Claim[] claims = [
            new(CustomClaims.AccountId, accountId.ToString())
        ];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
            );

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

        var refreshToken = new RefreshToken
        {
            AccountId = accountId,
            Token = GenerateRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(_options.ExpiresDays),
        };

        try
        {
            await tokenRepository.CreateRefreshToken(refreshToken);

            var jwtTokenValue = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new TokensDto(jwtTokenValue, refreshToken.Token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw new Exception("Произошла ошибка создания токенов");
        }
    }

    public async Task<Guid?> GetAccountIdByRefreshToken(string refreshToken)
    {
        return await tokenRepository.GetAccountIdByRefreshToken(refreshToken);
    }

    private static string GenerateRefreshToken()
    {
        var random = new SecureRandom();
        var randomBytes = new byte[32];
        random.NextBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
    
}