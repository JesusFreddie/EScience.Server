using EScience.Application.Requests;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
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
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
    {
        try
        {
            var tokens = await authService.Login(
                email: req.Email,
                password: req.Password
            );
            if (tokens is null)
                return BadRequest(AuthErrorMessage.InvalidDataLogin);

            HttpContext.Response.Cookies.Append("access-token", tokens.AccessToken);
            HttpContext.Response.Cookies.Append("refresh-token", tokens.RefreshToken);
        
            return Ok();
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Route("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        throw new NotSupportedException();
    }

    [Route("logout")]
    [HttpPost]
    public IActionResult Logout()
    {
        return Ok();
    }
    
}