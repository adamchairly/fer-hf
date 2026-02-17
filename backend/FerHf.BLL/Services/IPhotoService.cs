using FerHf.DAL.Entities;

namespace FerHf.BLL.Services;

public interface IPhotoService
{
    Task<List<Photo>> GetUserPhotosAsync(string userId, string? sortBy);
    Task<Photo> UploadAsync(string name, string userId, Stream fileStream, string fileName, string contentType);
    Task DeleteAsync(Guid id, string userId);
}
