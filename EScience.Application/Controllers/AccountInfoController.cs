using EScience.Application.Requests;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.Entities;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("account-info")]
public class AccountInfoController(
    ILogger<AccountInfoController> logger,
    IAccountInfoService accountInfoService
    ) : ControllerBase
{
    [HttpGet("get-all", Name = "AccountInfoGetAll")]
    public async Task<ActionResult<List<AccountInfo>>> GetAll()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            
            return await accountInfoService.GetAllAsync(id);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost(Name = "AccountInfoCreate")]
    public async Task<IActionResult> Create([FromBody] AccountInfoCreateRequest request)
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var id))
                return Unauthorized();
            
            await accountInfoService.CreateAsync(
                request.Field, request.Value, id);

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:guid}", Name = "AccountInfoUpdate")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AccountInfoCreateRequest request)
    {
        try
        {
            await accountInfoService.UpdateAsync(id, request.Field, request.Value);

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:guid}", Name = "AccountInfoDelete")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            await accountInfoService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}