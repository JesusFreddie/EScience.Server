using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction.Services;
using EScinece.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthController(IUserService userService): Controller
{
    private readonly IUserService _userService = userService;

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto data)
    {
        var user = new UserDto(Guid.Empty, Email: data.Email, Password: data.Password);
        
        var result = await _userService.Create(user);
        
        return result.Match<ActionResult<RegisterResponseDto>>(
            user =>
            {
                var response = new RegisterResponseDto(Email: user.Email);
                return Ok(response);
            }, 
            error => BadRequest(error)
            );
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