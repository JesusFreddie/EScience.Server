using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("account")]
public class AccountController(
    IAccountService accountService,
    ILogger<AccountController> logger
    ) : ControllerBase
{
    [HttpGet]
    [Route("session")]
    public async Task<ActionResult<ProfileDto>> GetProfileSession()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId == null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            
            var account = await accountService.GetProfile(id);
            if (account is null)
                return Unauthorized();

            return Ok(account);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    [Route("profile")]
    public async Task<ActionResult<ProfileDto>> GetProfile([FromQuery(Name = "id")] Guid id)
    {
        try
        {
            var account = await accountService.GetProfile(id);
            if (account is null)
                return NotFound();
            return Ok(account);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}