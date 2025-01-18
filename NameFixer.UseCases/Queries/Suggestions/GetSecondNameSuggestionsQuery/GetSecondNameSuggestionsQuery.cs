using NameFixer.Core.Repositories;
using NameFixer.UseCases.Helpers;

namespace NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;

public class GetSecondNameSuggestionsQuery(ISecondNameRepository repository) : IGetSecondNameSuggestionsQuery
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
            var newDistance = LevenshteinDistance.Calculate(name.SecondName.RemoveAccents(), firstName.RemoveAccents());

            if (newDistance >= results.Max(x => x.LevenshteinDistance)) continue;

            var pairToReplace = results.MaxBy(x => x.LevenshteinDistance);

            pairToReplace!.Name = name.SecondName;
            pairToReplace.LevenshteinDistance = newDistance;
            pairToReplace.Occurence = name.NumberOfOccurrences;
        }

        return results
            .OrderByDescending(x => x.Occurence)
            .Select(x => x.Name);
    }
}