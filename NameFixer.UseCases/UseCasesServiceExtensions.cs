using Microsoft.Extensions.DependencyInjection;
using NameFixer.Core.Models.DatasetModels;
using NameFixer.UseCases.Commands.InitializeFirstNamesCommand;
using NameFixer.UseCases.Commands.InitializeLastNamesCommand;
using NameFixer.UseCases.Commands.InitializeSecondNamesCommand;
using NameFixer.UseCases.Queries.GetNamesQuery;
using NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;

namespace NameFixer.UseCases;

public static class UseCasesServiceExtensions
{
    public static void AddUseCasesServices(this IServiceCollection services)
    {
        services.AddScoped<IGetFirstNameSuggestionsQuery, GetFirstNameSuggestionsQuery>();
        services.AddScoped<IGetSecondNameSuggestionsQuery, GetSecondNameSuggestionsQuery>();
        services.AddScoped<IGetLastNameSuggestionsQuery, GetLastNameSuggestionsQuery>();

        services.AddSingleton<IGetNamesQuery<FirstNameDatasetModel>, GetNamesQuery<FirstNameDatasetModel>>();
        services.AddSingleton<IGetNamesQuery<SecondNameDatasetModel>, GetNamesQuery<SecondNameDatasetModel>>();
        services.AddSingleton<IGetNamesQuery<LastNameDatasetModel>, GetNamesQuery<LastNameDatasetModel>>();

        services.AddSingleton<IInitializeFirstNamesCommand, InitializeFirstNamesCommand>();
        services.AddSingleton<IInitializeSecondNamesCommand, InitializeSecondNamesCommand>();
        services.AddSingleton<IInitializeLastNamesCommand, InitializeLastNamesCommand>();
    }
}