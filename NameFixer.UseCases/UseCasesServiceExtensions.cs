using Microsoft.Extensions.DependencyInjection;
using NameFixer.UseCases.Queries.GetFirstNameSuggestionsQuery;
using NameFixer.UseCases.Queries.GetLastNameSuggestionsQuery;
using NameFixer.UseCases.Queries.GetSecondNameSuggestionsQuery;

namespace NameFixer.UseCases;

public static class UseCasesServiceExtensions
{
    public static void AddUseCasesServices(this IServiceCollection services)
    {
        services.AddScoped<IGetFirstNameSuggestionsQuery, GetFirstNameSuggestionsQuery>();
        services.AddScoped<IGetSecondNameSuggestionsQuery, GetSecondNameSuggestionsQuery>();
        services.AddScoped<IGetLastNameSuggestionsQuery, GetLastNameSuggestionsQuery>();
    }
}