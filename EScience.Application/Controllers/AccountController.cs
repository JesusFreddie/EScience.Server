using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EScience.Application.Controllers;

[ApiController]
[Route("account")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AccountDto>> GetAccount([FromQuery(Name = "id")] Guid id)
    {
        AccountDto? account;
        try
        {
            account = await accountService.GetById(id);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        if (account is null)
            return NotFound();
        return account;
    }
}