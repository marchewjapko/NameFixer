namespace NameFixer.Core.Exceptions;

public class ServiceNotFoundException(string typeName) : Exception($"Service {typeName} was not found.");