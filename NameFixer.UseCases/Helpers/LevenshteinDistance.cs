namespace NameFixer.UseCases.Helpers;

public static class LevenshteinDistance
{
    /// <summary>
    ///     Calculates the Levenshtein distance between two strings.
    /// </summary>
    ///     <param name="source">The source string.</param>
    ///     <param name="target">The target string.</param>
    /// <returns>The Levenshtein distance between the two strings.</returns>
    public static int Calculate(string source, string target)
    {
        if (source.Length == 0) return target.Length;

        if (target.Length == 0) return source.Length;

        var levenshteinMatrix = new int[source.Length + 1, target.Length + 1];

        for (var i = 0; i <= source.Length; i++) levenshteinMatrix[i, 0] = i;

        for (var j = 0; j <= target.Length; j++) levenshteinMatrix[0, j] = j;

        for (var i = 1; i <= source.Length; i++)
        for (var j = 1; j <= target.Length; j++)
        {
            var cost = source[i - 1] == target[j - 1] ? 0 : 1;

            levenshteinMatrix[i, j] = Math.Min(
                Math.Min(levenshteinMatrix[i - 1, j] + 1, levenshteinMatrix[i, j - 1] + 1),
                levenshteinMatrix[i - 1, j - 1] + cost);
        }

        return levenshteinMatrix[source.Length, target.Length];
    }
}