using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace NameFixer.Infrastructure.Repositories.Configurations;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RepositoryOptions
{
    public const string FirstName = nameof(FirstNameRepository);
    public const string SecondName = nameof(SecondNameRepository);
    public const string LastName = nameof(LastNameRepository);

    /// <summary>
    ///     Minimum rate of occurrence the record needs to have in the dataset to be saved
    /// </summary>
    [Required]
    [Range(5, 10_000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int MinOccurrenceRate { get; init; }

    [Required]
    public required string FemaleLocalPath { get; init; }

    [Required]
    public required string MaleLocalPath { get; init; }

    public string? FemaleRemotePath { get; init; }

    public string? MaleRemotePath { get; init; }
}