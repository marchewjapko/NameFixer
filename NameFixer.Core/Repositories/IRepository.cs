namespace NameFixer.Core.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();

    void AddRange(params IEnumerable<T> entities);
}