using EScience.Application.Requests;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[AllowAnonymous]
[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService): ControllerBase
{
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto req)
    {
        var result = await authService.Register(
            email: req.Email, 
            password: req.Password, 
            name: req.Name
            );
        
        if (!result.onSuccess)
            return BadRequest(result.Error);
        
        return Ok();
    }
    
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
    {
        var token = await authService.Login(
            email: req.Email,
            password: req.Password
            );

        if (string.IsNullOrEmpty(token))
            return BadRequest(AuthErrorMessage.InvalidDataLogin);

        HttpContext.Response.Cookies.Append("access-token", token);
        
        return Ok();
    }

    [Route("logout")]
    [HttpPost]
    public IActionResult Logout()
    {
        return Ok();
    }
    
}