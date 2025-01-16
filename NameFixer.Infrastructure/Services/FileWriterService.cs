using System.IO.Abstractions;
using NameFixer.Core.ServicesInterfaces;

namespace NameFixer.Infrastructure.Services;

public class FileWriterService(IFileSystem fileSystem, IHttpClientFactory httpClientFactory) : IFileWriterService
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("FileWriterClient");

    public async Task WriteToFile(string remoteFilePath, string localFilePath)
    {
        await using var stream = await _client.GetStreamAsync(remoteFilePath);
        await using var fileStream = fileSystem.File.Create(localFilePath);
        await stream.CopyToAsync(fileStream);
    }
}