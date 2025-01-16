using System.IO.Abstractions;
using Microsoft.Extensions.Options;
using NameFixer.Core.Entities;
using NameFixer.Core.Exceptions;
using NameFixer.Core.Repositories;
using NameFixer.Core.ServicesInterfaces;
using NameFixer.Infrastructure.Models.DatasetFileModels;
using NameFixer.Infrastructure.Repositories.Configurations;

namespace NameFixer.Infrastructure.Repositories;

public class LastNameRepository(
    IOptionsMonitor<RepositoryOptions> options,
    IFileReaderService readService,
    IFileWriterService writeService,
    IFileSystem fileSystem) : ILastNameRepository
{
    private readonly HashSet<LastNameEntity> _cache = [];
    private readonly RepositoryOptions _options = options.Get(RepositoryOptions.LastName);

    public IEnumerable<LastNameEntity> GetAll()
    {
        return _cache.ToList();
    }

    public async Task Initialize()
    {
        var nameNames = await GetNames(_options.MaleLocalPath, _options.MaleRemotePath);
        var femaleNames = await GetNames(_options.FemaleLocalPath, _options.FemaleRemotePath);
        AddRange(nameNames.Where(x => x.NumberOfOccurrences >= _options.MinOccurrenceRate));
        AddRange(femaleNames.Where(x => x.NumberOfOccurrences >= _options.MinOccurrenceRate));
    }

    private void AddRange(params IEnumerable<LastNameDatasetModel> names)
    {
        foreach (var name in names)
        {
            var newName = new LastNameEntity(name.LastName, name.NumberOfOccurrences);

            _cache.Add(newName);
        }
    }

    private async Task<IEnumerable<LastNameDatasetModel>> GetNames(string localPath, string? remotePath)
    {
        if (fileSystem.File.Exists(localPath)) return readService.ReadCsvFile<LastNameDatasetModel>(localPath);

        if (remotePath == null) throw new LocalPathNotFoundRemotePathNotProvidedException(localPath);

        try
        {
            await writeService.WriteToFile(remotePath, localPath);

            return readService.ReadCsvFile<LastNameDatasetModel>(localPath);
        }
        catch (Exception e)
        {
            throw new DatasetDownloadProcessFailedException(remotePath, e);
        }
    }
}