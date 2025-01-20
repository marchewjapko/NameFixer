using Grpc.Core;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Helpers;
using NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;

namespace NameFixer.WebApi.Services;

public class SuggestionService(
    IGetFirstNameSuggestionsQuery firstNameSuggestionsQuery,
    IGetSecondNameSuggestionsQuery secondNameSuggestionsQuery,
    IGetLastNameSuggestionsQuery lastNameSuggestionsQuery) : SuggestionsService.SuggestionsServiceBase
{
    public override Task<GetSuggestionsResponse> GetFirstNameSuggestions(
        GetSuggestionsRequest request,
        ServerCallContext context)
    {
        var names = firstNameSuggestionsQuery.Handle(request.Key.ToUpper());

        var response = new GetSuggestionsResponse();

        response.Suggestions.AddRange(names.Select(x => x.Capitalize()));

        return Task.FromResult(response);
    }

    public override Task<GetSuggestionsResponse> GetSecondNameSuggestions(
        GetSuggestionsRequest request,
        ServerCallContext context)
    {
        var names = secondNameSuggestionsQuery.Handle(request.Key.ToUpper());

        var response = new GetSuggestionsResponse();

        response.Suggestions.AddRange(names.Select(x => x.Capitalize()));

        return Task.FromResult(response);
    }

    public override Task<GetSuggestionsResponse> GetLastNameSuggestions(
        GetSuggestionsRequest request,
        ServerCallContext context)
    {
        var names = lastNameSuggestionsQuery.Handle(request.Key.ToUpper());

        var response = new GetSuggestionsResponse();

        response.Suggestions.AddRange(names.Select(x => x.Capitalize()));

        return Task.FromResult(response);
    }
}