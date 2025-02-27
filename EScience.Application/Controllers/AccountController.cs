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
    public async Task<ActionResult<AccountDto>> GetAccountSession()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId == null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            
            var account = await accountService.GetById(id);
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
    public async Task<ActionResult<AccountDto>> GetAccount([FromQuery(Name = "id")] Guid id)
    {
        try
        {
            var account = await accountService.GetById(id);
            
            if (account is null)
                return NotFound();
            return account;
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}