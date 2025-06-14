using EScinece.Domain.Abstraction.Services;
using EScinece.Infrastructure.Services;

namespace EScience.Application.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IArticleParticipantService, ArticleParticipantService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticleBranchService, ArticleBranchService>();
        services.AddScoped<IArticleVersionService, ArticleVersionService>();
        services.AddScoped<IMergeService, MergeService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IArticleFavoriteService, ArticleFavoriteService>();
        services.AddScoped<IAccountInfoService, AccountInfoService>();
        
        return services;
    }
}