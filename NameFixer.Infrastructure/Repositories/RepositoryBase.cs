using NameFixer.Core.Exceptions.CustomExceptions;
using NameFixer.Core.Repositories;

namespace NameFixer.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepository<T>
{
    private readonly HashSet<T> _cache = [];

    public IEnumerable<T> GetAll()
    {
        if (_cache.Count == 0) throw new NotInitializedException(typeof(T).FullName!);

        return _cache.ToList();
    }

    public void AddRange(params IEnumerable<T> entities)
    {
        foreach (var entity in entities) _cache.Add(entity);
    }
}