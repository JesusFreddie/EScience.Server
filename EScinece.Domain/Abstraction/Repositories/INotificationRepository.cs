using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.Repositories;

public interface INotificationRepository
{
    public Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid accountId);
    public Task<Notification> Create(Notification notification);
    public Task MarkAsReadAsync(int notificationId);
    public Task MarkAsReadAllAsync(Guid accountId);
    public Task<int> GetCountNotificationsAsync(Guid accountId);
}