using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[ApiController]
[Route("account")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<AccountDto>> GetAccount([FromQuery(Name = "id")] Guid id)
    {
        var account = await _accountService.GetById(id);
        return Ok(account);
    }
}