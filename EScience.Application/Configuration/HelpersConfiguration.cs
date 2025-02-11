using EScinece.Domain.Abstraction.Helpers;
using EScinece.Infrastructure.Helpers;

namespace EScience.Application.Configuration;

public static class HelpersConfiguration
{
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
}