using Microsoft.Extensions.Options;
using NameFixer.Core.Entities;
using NameFixer.Core.Models.DatasetModels;
using NameFixer.Core.Repositories;
using NameFixer.UseCases.Options;
using NameFixer.UseCases.Queries.GetNamesQuery;

namespace NameFixer.UseCases.Commands.InitializeFirstNamesCommand;

public class InitializeFirstNamesCommand(
    IFirstNameRepository repository,
    IOptionsMonitor<RepositoryOptions> options,
    IGetNamesQuery<FirstNameDatasetModel> getNamesQuery) : IInitializeFirstNamesCommand
{
    private readonly RepositoryOptions _options = options.Get(RepositoryOptions.FirstName);

    public async Task Handle()
    {
        var maleNames = await getNamesQuery.Handle(_options.MaleLocalPath, _options.MaleRemotePath);
        var femaleNames = await getNamesQuery.Handle(_options.FemaleLocalPath, _options.FemaleRemotePath);

        repository.AddRange(
            maleNames
                .Where(x => x.NumberOfOccurrences >= _options.MinOccurrenceRate)
                .Select(x => new FirstNameEntity(x.FirstName, x.NumberOfOccurrences)));

        repository.AddRange(
            femaleNames
                .Where(x => x.NumberOfOccurrences >= _options.MinOccurrenceRate)
                .Select(x => new FirstNameEntity(x.FirstName, x.NumberOfOccurrences)));
    }
}