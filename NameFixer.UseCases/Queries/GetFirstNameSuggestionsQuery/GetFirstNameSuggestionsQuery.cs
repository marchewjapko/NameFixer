using NameFixer.Core.Repositories;
using NameFixer.UseCases.Helpers;

namespace NameFixer.UseCases.Queries.GetFirstNameSuggestionsQuery;

public class GetFirstNameSuggestionsQuery(IFirstNameRepository repository) : IGetFirstNameSuggestionsQuery
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
            var newDistance = LevenshteinDistance.Calculate(name.FirstName, firstName);

            if (newDistance >= results.Max(x => x.LevenshteinDistance)) continue;

            var pairToReplace = results.MaxBy(x => x.LevenshteinDistance);

            pairToReplace!.Name = name.FirstName;
            pairToReplace.LevenshteinDistance = newDistance;
            pairToReplace.Occurence = name.OccurenceRate;
        }

        return results
            .OrderByDescending(x => x.Occurence)
            .Select(x => x.Name);
    }
}