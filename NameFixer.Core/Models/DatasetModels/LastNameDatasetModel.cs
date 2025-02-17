using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace NameFixer.Core.Models.DatasetModels;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record LastNameDatasetModel
{
    [Name("Nawisko aktualne")]
    public required string LastName { get; init; }

    [Name("Liczba")]
    public int NumberOfOccurrences { get; init; }
}