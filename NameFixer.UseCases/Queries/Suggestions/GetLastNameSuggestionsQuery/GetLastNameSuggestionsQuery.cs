using NameFixer.Core.Repositories;
using NameFixer.UseCases.Helpers;

namespace NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;

public class GetLastNameSuggestionsQuery(ILastNameRepository repository) : IGetLastNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName)
    {
        const int suggestionsCount = 5;

        var results = Enumerable
            .Range(1, suggestionsCount)
            .Select(_ => new NameOccurencePair())
            .ToArray();

        foreach (var name in repository.GetAll())
        {
            var newDistance = LevenshteinDistance.Calculate(name.LastName.RemoveAccents(), firstName.RemoveAccents());

            if (newDistance >= results.Max(x => x.LevenshteinDistance)) continue;

            var pairToReplace = results.MaxBy(x => x.LevenshteinDistance);

            pairToReplace!.Name = name.LastName;
            pairToReplace.LevenshteinDistance = newDistance;
            pairToReplace.Occurence = name.NumberOfOccurrences;
        }

        return results
            .OrderByDescending(x => x, new NameOccurencePairComparer())
            .Select(x => x.Name);
    }
}