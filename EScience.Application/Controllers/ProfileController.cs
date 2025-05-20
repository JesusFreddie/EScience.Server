using System.ComponentModel.DataAnnotations;
using EScinece.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace EScience.Application.Controllers;

[Route("profile")]
[Authorize]
[ApiController]
public class ProfileController(
    ILogger<ProfileController> logger
    ) : ControllerBase
{
    [HttpPost("avatar", Name = "ProfileUploadAvatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile? file)
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var userId))
                return Unauthorized();
            
            if (userId == Guid.Empty)
                return Unauthorized();
            
            if (file is null)
                return BadRequest("File no uploaded");
            if (file.Length == 0)
                return BadRequest("File no uploaded");
            
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                return BadRequest("Invalid file extension");
            
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "avatars");
            
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            
            var fileName = $"{userId}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Ok(new
            {
                FileName = fileName,
                UserId = userId,
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("avatars/{userId:guid}", Name = "ProfileGetAvatars")]
    public ActionResult<FileStreamResult> GetProfileAvatar(Guid userId)
    {
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "avatars", userId.ToString());
            
            if (!System.IO.File.Exists(filePath))
                return NotFound("Avatar not found");

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            
            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, contentType);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("avatars/session", Name = "ProfileGetAvatarSession")]
    public ActionResult<FileStreamResult> GetProfileAvatarSession()
    {
        try
        {
            var accountId = User.Claims.FirstOrDefault(claim => claim.Type == CustomClaims.AccountId);
            if (accountId is null || !Guid.TryParse(accountId.Value, out var userId))
                return Unauthorized();
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "avatars", userId.ToString());
            
            if (!System.IO.File.Exists(filePath))
                return NotFound("Avatar not found");

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            
            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, contentType);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }
}