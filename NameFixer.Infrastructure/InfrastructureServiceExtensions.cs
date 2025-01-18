using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NameFixer.Core.Repositories;
using NameFixer.Core.ServicesInterfaces;
using NameFixer.Infrastructure.Repositories;
using NameFixer.Infrastructure.Services;

namespace NameFixer.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IFirstNameRepository, FirstNameRepository>();
        services.AddSingleton<ISecondNameRepository, SecondNameRepository>();
        services.AddSingleton<ILastNameRepository, LastNameRepository>();

        services.AddSingleton<IFileReaderService, FileReaderService>();
        services.AddSingleton<IFileWriterService, FileWriterService>();

        services.AddSingleton<IFileSystem, FileSystem>();

        services.AddHttpClient();
    }
}