using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace NameFixer.UseCases.Options;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public record RepositoryOptions
{
    public const string FirstName = "FirstNameOptions";
    public const string SecondName = "SecondNameOptions";
    public const string LastName = "LastNameOptions";

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