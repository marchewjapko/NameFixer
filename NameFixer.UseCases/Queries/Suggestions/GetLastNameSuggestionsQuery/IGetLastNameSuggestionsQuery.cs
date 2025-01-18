namespace NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;

public interface IGetLastNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName);
}