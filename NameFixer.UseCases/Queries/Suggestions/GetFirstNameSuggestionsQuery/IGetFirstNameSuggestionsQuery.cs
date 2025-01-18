namespace NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;

public interface IGetFirstNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName);
}