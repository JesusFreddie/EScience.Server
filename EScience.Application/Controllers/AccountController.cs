using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("account")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<AccountDto>> GetAccount([FromQuery(Name = "id")] Guid id)
    {
        var account = await accountService.GetById(id);
        if (account is null)
            return NotFound();
        return account;
    }
}