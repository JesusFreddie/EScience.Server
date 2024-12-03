
using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
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
    public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto data)
    {
        var user = new UserDto(Email: data.Email, Password: data.Password);
        
        var result = await _userService.Create(user);
        
        return result.Match<ActionResult<RegisterResponseDto>>(user => Ok(user), error => BadRequest(error));
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