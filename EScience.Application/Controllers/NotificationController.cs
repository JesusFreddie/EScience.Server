using EScience.Application.Requests;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Authorize]
[Route("notification")]
public class NotificationController(
    INotificationService notificationService,
    ILogger<NotificationController> logger
    ) : ControllerBase
{
    [HttpGet(Name = "NotificationGet")]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();

            var notifications = await notificationService.GetNotificationsAsync(id);

            return Ok(notifications);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("read/{id:int}", Name = "NotificationMarkRead")]
    public async Task<IActionResult> MarkRead(int id)
    {
        try
        {
            await notificationService.MarkAsReadAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("read/all", Name = "NotificationMarkReadAll")]
    public async Task<IActionResult> MarkReadAll()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            await notificationService.MarkAsReadAllAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("count-unread", Name = "NotificationCountUnread")]
    public async Task<ActionResult<EntityCount>> GetCountUnreadNotifications()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();

            var result = await notificationService.CountUnreadNotificationsAsync(id);

            return Ok(new EntityCount(result));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}