using EScience.Application.Configuration;
using EScinece.Domain.Abstraction.Repositories;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EScinece.Infrastructure.Services;

public class NotificationService(
    IHubContext<NotificationHub> hubContext,
    INotificationRepository notificationRepository,
    ILogger<NotificationService> logger
    ) : INotificationService
{
    public async Task SendNotification(Guid accountId, NotificationType type, string title, string message)
    {
        try
        {
            var notification = new Notification
            {
                Title = title,
                AccountId = accountId,
                Type = type,
                IsRead = false,
                Message = message,
            };

            try
            {
                await notificationRepository.Create(notification);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw new Exception($"An error occured while sending notification: {e.Message}");
            }

            await hubContext.Clients.User(accountId.ToString())
                .SendAsync("NewNotification", notification);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        try
        {
            await notificationRepository.MarkAsReadAsync(notificationId);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception($"An error occured while marking notification as read: {e.Message}");
        }
    }

    public async Task<IEnumerable<Notification>> GetNotificationsAsync(Guid accountId)
    {
        try
        {
            return await notificationRepository.GetAllNotificationsAsync(accountId);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception($"An error occured while getting notifications: {e.Message}");
        }
    }

    public async Task<int> CountUnreadNotificationsAsync(Guid accountId)
    {
        try
        {
            return await notificationRepository.GetCountNotificationsAsync(accountId);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new Exception($"An error occured while count unread notifications: {e.Message}");
        }
    }
}