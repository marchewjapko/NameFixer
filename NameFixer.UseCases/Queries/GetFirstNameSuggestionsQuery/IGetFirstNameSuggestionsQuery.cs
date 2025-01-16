namespace NameFixer.UseCases.Queries.GetFirstNameSuggestionsQuery;

public interface IGetFirstNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName);
}