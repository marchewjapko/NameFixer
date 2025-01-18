namespace NameFixer.UseCases.Queries.GetNamesQuery;

public interface IGetNamesQuery<T>
{
    public Task<IEnumerable<T>> Handle(string localPath, string? remotePath = null);
}