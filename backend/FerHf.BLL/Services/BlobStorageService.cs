using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace FerHf.BLL.Services;

public partial class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly string? _publicBaseUrl;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["BlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("BlobStorage:ConnectionString is not configured.");
        var containerName = configuration["BlobStorage:ContainerName"]
            ?? throw new InvalidOperationException("BlobStorage:ContainerName is not configured.");

        _publicBaseUrl = configuration["BlobStorage:PublicBaseUrl"];

        var serviceClient = new BlobServiceClient(connectionString);
        _containerClient = serviceClient.GetBlobContainerClient(containerName);

        //
    }

    public async Task<string> UploadAsync(string userId, string fileName, Stream content, string contentType)
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var normalizedName = NormalizeName(fileName);
        var blobPath = $"{userId}/{normalizedName}";
        var blobClient = _containerClient.GetBlobClient(blobPath);

        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });

        if (!string.IsNullOrEmpty(_publicBaseUrl))
            return $"{_publicBaseUrl.TrimEnd('/')}/{_containerClient.Name}/{blobPath}";

        return blobClient.Uri.ToString();
    }

    public async Task DeleteAsync(string userId, string fileName)
    {
        var normalizedName = NormalizeName(fileName);
        var blobPath = $"{userId}/{normalizedName}";
        var blobClient = _containerClient.GetBlobClient(blobPath);

        await blobClient.DeleteIfExistsAsync();
    }

    private static string NormalizeName(string name)
    {
        var normalized = name.ToLowerInvariant().Trim();
        normalized = normalized.Replace(" ", "-");
        normalized = SpecialCharsRegex().Replace(normalized, "");
        normalized = MultipleDashesRegex().Replace(normalized, "-");
        return normalized.Trim('-');
    }

    [GeneratedRegex("[^a-z0-9\\-.]")]
    private static partial Regex SpecialCharsRegex();

    [GeneratedRegex("-{2,}")]
    private static partial Regex MultipleDashesRegex();
}
