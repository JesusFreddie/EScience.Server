using EScinece.Domain.Abstraction.Repositories;
using EScinece.Infrastructure.Repositories;

namespace EScience.Application.Configuration;

public static class RepositoryConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IArticleParticipantRepository, ArticleParticipantRepository>();
        services.AddScoped<IArticleBranchRepository, ArticleBranchRepository>();
        services.AddScoped<IArticleVersionRepository, ArticleVersionRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IArticleFavoriteRepository, ArticleFavoriteRepository>();
        services.AddScoped<IAccountInfoRepository, AccountInfoRepository>();
        return services;
    }
}