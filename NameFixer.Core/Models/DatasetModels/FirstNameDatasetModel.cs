using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace NameFixer.Core.Models.DatasetModels;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record FirstNameDatasetModel
{
    [Name("IMIĘ_PIERWSZE")]
    public required string FirstName { get; init; }

    [Name("LICZBA_WYSTĄPIEŃ")]
    public int NumberOfOccurrences { get; init; }
}