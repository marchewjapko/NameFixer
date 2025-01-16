using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;

namespace NameFixer.Infrastructure.Models.DatasetFileModels;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record SecondNameDatasetModel
{
    [Name("IMIĘ_DRUGIE")]
    public required string SecondName { get; init; }

    [Name("LICZBA_WYSTĄPIEŃ")]
    public int NumberOfOccurrences { get; init; }
}