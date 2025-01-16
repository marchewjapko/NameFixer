namespace NameFixer.UseCases.Helpers;

public class NameOccurencePair
{
    public string Name { get; set; } = string.Empty;

    public int LevenshteinDistance { get; set; } = int.MaxValue;

    public int Occurence { get; set; }
}