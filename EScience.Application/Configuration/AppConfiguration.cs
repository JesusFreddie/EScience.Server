namespace EScience.Application.Configuration;

public static class AppConfiguration
{
    public static WebApplication AddNotificationHub(this WebApplication app)
    {
        app.MapHub<NotificationHub>("hubs/notification");
        return app;
    }
}