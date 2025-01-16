namespace NameFixer.Core.ServicesInterfaces;

public interface IFileReaderService
{
    public IEnumerable<T> ReadCsvFile<T>(string filePath);
}