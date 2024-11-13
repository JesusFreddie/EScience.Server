using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthController: Controller
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult<User?>> Register([FromBody] UserRegisterDto data)
    {
        var result = await _userService.Register(data);
        
        return result.Match<ActionResult<User?>>((user) => Ok(), error => BadRequest(error));
    }
    
    [Route("login")]
    [HttpPost]
    public IActionResult Login()
    {
        return Ok();
    }

    [Route("logout")]
    [HttpPost]
    public IActionResult Logout()
    {
        return Ok();
    }
    
}