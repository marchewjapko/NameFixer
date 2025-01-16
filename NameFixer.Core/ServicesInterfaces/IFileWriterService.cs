namespace NameFixer.Core.ServicesInterfaces;

public interface IFileWriterService
{
    Task WriteToFile(string remoteFilePath, string localFilePath);
}