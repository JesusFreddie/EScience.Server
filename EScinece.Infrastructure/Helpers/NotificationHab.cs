using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EScience.Application.Configuration;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var accountId = GetCurrentAccountId();
        await Groups.AddToGroupAsync(Context.ConnectionId, accountId.ToString());
        // await SendPendingNotifications(accountId);
        await base.OnConnectedAsync();
    }

    private Guid GetCurrentAccountId()
    {
        return Guid.Parse(Context.UserIdentifier);
    }

    private async Task SendPendingNotifications(Notification notification)
    {
        // Реализация будет добавлена после создания NotificationService
    }
}