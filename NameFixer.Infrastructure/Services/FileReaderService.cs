using System.Globalization;
using System.IO.Abstractions;
using CsvHelper;
using NameFixer.Core.ServicesInterfaces;

namespace NameFixer.Infrastructure.Services;

public class FileReaderService(IFileSystem fileSystem) : IFileReaderService
{
    public IEnumerable<T> ReadCsvFile<T>(string filePath)
    {
        using Stream stream = fileSystem.File.OpenRead(filePath);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>();

        return records.ToList();
    }
}