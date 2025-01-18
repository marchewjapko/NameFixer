using System.IO.Abstractions;
using NameFixer.Core.ServicesInterfaces;

namespace NameFixer.UseCases.Queries.GetNamesQuery;

public class GetNamesQuery<T>(
    IFileWriterService writerService,
    IFileReaderService readerService,
    IFileSystem fileSystem) : IGetNamesQuery<T>
{
    public async Task<IEnumerable<T>> Handle(string localPath, string? remotePath = null)
    {
        if (fileSystem.File.Exists(localPath)) return readerService.ReadCsvFile<T>(localPath);

        if (remotePath == null)
            throw new ArgumentException(
                $"Path to file '{localPath}' has not been found, remote path has not been provided.");

        await writerService.WriteToFile(remotePath, localPath);

        return readerService.ReadCsvFile<T>(localPath);
    }
}