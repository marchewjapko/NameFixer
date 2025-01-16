namespace NameFixer.Core.Exceptions;

public class DatasetDownloadProcessFailedException(string remotePath, Exception ex)
    : Exception($"Downloading file from '{remotePath}' failed.", ex);