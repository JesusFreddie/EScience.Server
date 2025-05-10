using System.Security.Claims;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EScience.Application.Configuration;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var accountId = GetAccountId(Context.User);
        await Groups.AddToGroupAsync(Context.ConnectionId, accountId.ToString());
        await base.OnConnectedAsync();
    }

    private static Guid GetAccountId(ClaimsPrincipal user)
    {
        var accountIdClaim = user.FindFirst(CustomClaims.AccountId);
        if (accountIdClaim == null || !Guid.TryParse(accountIdClaim.Value, out var accountId))
        {
            throw new InvalidOperationException("User account ID not found");
        }
        return accountId;
    }
}