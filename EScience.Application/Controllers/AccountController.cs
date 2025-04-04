using EScience.Application.Responses;
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
            {
                return Unauthorized();
            }
            
            var profile = await accountService.GetProfile(id);
            if (profile is null)
            {
                return Unauthorized();
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet]
    [Route("{accountName}")]
    public async Task<ActionResult<ProfileDto>> GetProfile(string accountName)
    {
        try
        {
            var profile = await accountService.GetProfile(accountName);
            if (profile is null)
                return NotFound();
            return Ok(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}