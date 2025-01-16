using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using NameFixer.Core.Exceptions;
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

    public static async Task InitializeRepositories(this IServiceProvider serviceProvider)
    {
        var firstNamesTask = InitializeFirstNames(serviceProvider);
        var secondNamesTask = InitializeSecondNames(serviceProvider);
        var lastNamesTask = InitializeLastNames(serviceProvider);

        await Task.WhenAll(firstNamesTask, secondNamesTask, lastNamesTask);
    }

    private static Task InitializeFirstNames(IServiceProvider serviceProvider)
    {
        var firstNameRepository = serviceProvider.GetService<IFirstNameRepository>();

        if (firstNameRepository is null)
            throw new ServiceNotFoundException(nameof(IFirstNameRepository));

        return firstNameRepository.Initialize();
    }

    private static Task InitializeSecondNames(IServiceProvider serviceProvider)
    {
        var secondNameRepository = serviceProvider.GetService<ISecondNameRepository>();

        if (secondNameRepository is null)
            throw new ServiceNotFoundException(nameof(ISecondNameRepository));

        return secondNameRepository.Initialize();
    }

    private static Task InitializeLastNames(IServiceProvider serviceProvider)
    {
        var lastNameRepository = serviceProvider.GetService<ILastNameRepository>();

        if (lastNameRepository is null)
            throw new ServiceNotFoundException(nameof(ILastNameRepository));

        return lastNameRepository.Initialize();
    }
}