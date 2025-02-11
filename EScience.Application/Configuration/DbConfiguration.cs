using EScinece.Infrastructure.Data;

namespace EScience.Application.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddDbNpgsql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnectionFactory, EScienceDbContext>(provider => 
            new EScienceDbContext(
                provider.GetRequiredService<ILogger<EScienceDbContext>>(),
                configuration.GetConnectionString(nameof(EScienceDbContext))!));
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6380");
        return services;
    }
}