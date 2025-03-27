using EScience.Application.Requests;
using EScience.Application.Responses;
using EScinece.Domain.Abstraction.ErrorMessages;
using EScinece.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EScience.Application.Controllers;

[AllowAnonymous]
[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    ILogger<AuthController> logger
    ): ControllerBase
{
    [Route("register")]
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDto req)
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
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginRequestDto req)
    {
        string token;
        try
        {
            token = await authService.Login(
                email: req.Email,
                password: req.Password
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal Server Error");
        }

        if (string.IsNullOrEmpty(token))
            return BadRequest(AuthErrorMessage.InvalidDataLogin);

        HttpContext.Response.Cookies.Append("access-token", token);
        
        return Ok("Login successful");
    }

    [Route("logout")]
    [HttpPost]
    public IActionResult Logout()
    {
        return Ok();
    }
    
}