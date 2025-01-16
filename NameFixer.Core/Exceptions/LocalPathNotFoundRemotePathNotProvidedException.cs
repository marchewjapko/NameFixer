namespace NameFixer.Core.Exceptions;

public class LocalPathNotFoundRemotePathNotProvidedException(string localPath) : Exception(
    $"Path to file '{localPath}' has not been found, remote path has not been provided.");