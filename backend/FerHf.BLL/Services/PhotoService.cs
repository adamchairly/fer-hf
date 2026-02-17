using FerHf.DAL;
using FerHf.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FerHf.BLL.Services;

public class PhotoService : IPhotoService
{
    private readonly ApplicationDbContext _context;
    private readonly IBlobStorageService _blobService;

    public PhotoService(ApplicationDbContext context, IBlobStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    public async Task<List<Photo>> GetUserPhotosAsync(string userId, string? sortBy)
    {
        var query = _context.Photos.Where(p => p.UserId == userId);

        query = sortBy?.ToLowerInvariant() switch
        {
            "name" => query.OrderBy(p => p.Name),
            "date" => query.OrderByDescending(p => p.Date),
            _ => query.OrderByDescending(p => p.Date)
        };

        return await query.ToListAsync();
    }

    public async Task<Photo> UploadAsync(string name, string userId, Stream fileStream, string fileName, string contentType)
    {
        var url = await _blobService.UploadAsync(userId, fileName, fileStream, contentType);

        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            Name = name,
            Url = url,
            Date = DateTime.UtcNow,
            UserId = userId
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return photo;
    }

    public async Task DeleteAsync(Guid id, string userId)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId)
            ?? throw new InvalidOperationException("Photo not found or access denied.");

        await _blobService.DeleteAsync(userId, photo.Url.Split('/').Last());

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();
    }
}
