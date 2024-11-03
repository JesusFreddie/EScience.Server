using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthController: Controller
{
    public AuthController()
    {
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }

    [Route("logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        return Ok();
    }
    
}