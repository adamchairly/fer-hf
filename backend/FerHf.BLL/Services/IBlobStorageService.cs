namespace FerHf.BLL.Services;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string userId, string fileName, Stream content, string contentType);
    Task DeleteAsync(string userId, string fileName);
}
