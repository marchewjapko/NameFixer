namespace NameFixer.UseCases.Queries.GetSecondNameSuggestionsQuery;

public interface IGetSecondNameSuggestionsQuery
{
    public IEnumerable<string> Handle(string firstName);
}