namespace EScience.Application.Configuration;

public static class NotificationConfiguration
{
    public static WebApplication AddNotificationHub(this WebApplication app)
    {
        app.MapHub<NotificationHub>("hubs/notification").RequireCors("NuxtCorsPolicy");
        return app;
    }

    public static IServiceCollection AddSignal(this IServiceCollection builder)
    {
        builder.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });
        return builder;
    }
}