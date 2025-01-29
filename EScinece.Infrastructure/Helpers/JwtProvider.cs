using EScinece.Domain.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Helpers;
public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    public string GenerateToken(Guid userId)
    {
        Claim[] claims = [new("userId", userId.ToString())];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256
            );

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpitesHours)
            );
        
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenValue;
    }
}