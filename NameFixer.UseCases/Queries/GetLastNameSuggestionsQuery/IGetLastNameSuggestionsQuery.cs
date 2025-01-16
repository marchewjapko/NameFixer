namespace NameFixer.UseCases.Queries.GetLastNameSuggestionsQuery;

public interface IGetLastNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName);
}