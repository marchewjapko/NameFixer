using Microsoft.Extensions.Options;
using NameFixer.Core.Entities;
using NameFixer.Core.Models.DatasetModels;
using NameFixer.Core.Repositories;
using NameFixer.UseCases.Options;
using NameFixer.UseCases.Queries.GetNamesQuery;

namespace NameFixer.UseCases.Commands.InitializeLastNamesCommand;

public class InitializeLastNamesCommand(
    ILastNameRepository repository,
    IOptionsMonitor<RepositoryOptions> options,
    IGetNamesQuery<LastNameDatasetModel> getNamesQuery) : IInitializeLastNamesCommand
{
    private readonly RepositoryOptions _options = options.Get(RepositoryOptions.LastName);

    public async Task Handle()
    {
        var nameNames = await getNamesQuery.Handle(_options.MaleLocalPath, _options.MaleRemotePath);
        var femaleNames = await getNamesQuery.Handle(_options.FemaleLocalPath, _options.FemaleRemotePath);

        repository.AddRange(
            nameNames
                .Where(x => x.NumberOfOccurrences >= _options.MinOccurrenceRate)
                .Select(x => new LastNameEntity(x.LastName, x.NumberOfOccurrences)));

        repository.AddRange(
            femaleNames
                .Where(x => x.NumberOfOccurrences >= _options.MinOccurrenceRate)
                .Select(x => new LastNameEntity(x.LastName, x.NumberOfOccurrences)));
    }
}