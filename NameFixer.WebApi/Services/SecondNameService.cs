using Grpc.Core;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Helpers;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;

namespace NameFixer.WebApi.Services;

public class SecondNameService(IGetSecondNameSuggestionsQuery suggestionsQuery)
    : gRPCServices.SecondNameService.SecondNameServiceBase
{
    public override Task<GetSecondNameSuggestionsResponse> GetSecondNameSuggestions(
        GetSecondNameSuggestionsRequest request,
        ServerCallContext context)
    {
        var names = suggestionsQuery.Handle(request.SecondName.ToUpper());

        var response = new GetSecondNameSuggestionsResponse();

        response.SecondNameSuggestions.AddRange(names.Select(x => x.Capitalize()));

        return Task.FromResult(response);
    }
}