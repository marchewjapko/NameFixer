using NameFixer.UseCases.Commands.InitializeFirstNamesCommand;
using NameFixer.UseCases.Commands.InitializeLastNamesCommand;
using NameFixer.UseCases.Commands.InitializeSecondNamesCommand;

namespace NameFixer.WebApi.Configurations;

public static class InitializationConfiguration
{
    public static async Task InitializeStores(this IServiceProvider serviceProvider)
    {
        var firstNamesTask = InitializeFirstNames(serviceProvider);
        var secondNamesTask = InitializeSecondNames(serviceProvider);
        var lastNamesTask = InitializeLastNames(serviceProvider);

        await Task.WhenAll(firstNamesTask, secondNamesTask, lastNamesTask);
    }

    private static Task InitializeFirstNames(IServiceProvider serviceProvider)
    {
        var command = serviceProvider.GetService<IInitializeFirstNamesCommand>();

        if (command is null)
            throw new ArgumentException($"Service '{nameof(IInitializeFirstNamesCommand)}' was not found.");

        return command.Handle();
    }

    private static Task InitializeSecondNames(IServiceProvider serviceProvider)
    {
        var command = serviceProvider.GetService<IInitializeSecondNamesCommand>();

        if (command is null)
            throw new ArgumentException($"Service '{nameof(IInitializeSecondNamesCommand)}' was not found.");

        return command.Handle();
    }

    private static Task InitializeLastNames(IServiceProvider serviceProvider)
    {
        var command = serviceProvider.GetService<IInitializeLastNamesCommand>();

        if (command is null)
            throw new ArgumentException($"Service '{nameof(IInitializeLastNamesCommand)}' was not found.");

        return command.Handle();
    }
}