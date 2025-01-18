using Grpc.Core;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Helpers;
using NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;

namespace NameFixer.WebApi.Services;

public class LastNameService(IGetLastNameSuggestionsQuery suggestionsQuery)
    : gRPCServices.LastNameService.LastNameServiceBase
{
    public override Task<GetLastNameSuggestionsResponse> GetLastNameSuggestions(
        GetLastNameSuggestionsRequest request,
        ServerCallContext context)
    {
        var names = suggestionsQuery.Handle(request.LastName.ToUpper());

        var response = new GetLastNameSuggestionsResponse();

        response.LastNameSuggestions.AddRange(names.Select(x => x.Capitalize()));

        return Task.FromResult(response);
    }
}