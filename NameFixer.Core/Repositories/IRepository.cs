namespace NameFixer.Core.Repositories;

public interface IRepository<out T> where T : class
{
    IEnumerable<T> GetAll();

    Task Initialize();
}