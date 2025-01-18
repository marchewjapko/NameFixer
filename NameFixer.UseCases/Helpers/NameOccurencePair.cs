namespace NameFixer.UseCases.Helpers;

public class NameOccurencePair
{
    public string Name { get; set; } = string.Empty;

    public int LevenshteinDistance { get; set; } = int.MaxValue;

    public int Occurence { get; set; }
}

public class NameOccurencePairComparer : IComparer<NameOccurencePair>
{
    public int Compare(NameOccurencePair? x, NameOccurencePair? y)
    {
        switch (x)
        {
            case null when y is null:
                return 0;

            case null:
                return -1;
        }

        if (y is null) return 1;

        if (x.LevenshteinDistance == 0) return 1;

        if (y.LevenshteinDistance == 0) return -1;

        if (x.LevenshteinDistance == y.LevenshteinDistance) return x.Occurence - y.Occurence;

        return y.LevenshteinDistance - x.LevenshteinDistance;
    }
}