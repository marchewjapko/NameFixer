namespace NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;

public interface IGetSecondNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName);
}