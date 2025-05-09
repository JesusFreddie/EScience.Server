using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Services;

public interface INotificationService
{
    Task SendNotification(Guid accountId, NotificationType type, string message);
    Task MarkAsReadAsync(int notificationId);
    Task<IEnumerable<Notification>> GetNotificationsAsync(Guid accountId);
}