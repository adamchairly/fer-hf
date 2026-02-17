using System.Security.Claims;
using FerHf.API.DTOs;
using FerHf.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FerHf.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;

    public PhotoController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? sort)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var photos = await _photoService.GetUserPhotosAsync(userId, sort);

        var result = photos.Select(p => new PhotoDto
        {
            Id = p.Id,
            Name = p.Name,
            Url = p.Url,
            Date = p.Date.ToString("yyyy-MM-dd HH:mm")
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] string name, IFormFile file)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 40)
            return BadRequest(new { message = "Photo name is required and must be at most 40 characters." });

        if (file == null || file.Length == 0)
            return BadRequest(new { message = "File is required." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        using var stream = file.OpenReadStream();
        var photo = await _photoService.UploadAsync(name, userId, stream, file.FileName, file.ContentType);

        return Ok(new PhotoDto
        {
            Id = photo.Id,
            Name = photo.Name,
            Url = photo.Url,
            Date = photo.Date.ToString("yyyy-MM-dd HH:mm")
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await _photoService.DeleteAsync(id, userId);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
