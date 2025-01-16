using Grpc.Core;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Helpers;
using NameFixer.UseCases.Queries.GetFirstNameSuggestionsQuery;

namespace NameFixer.WebApi.Services;

public class FirstNameService(IGetFirstNameSuggestionsQuery suggestionsQuery)
    : gRPCServices.FirstNameService.FirstNameServiceBase
{
    public override Task<GetFirstNameSuggestionsResponse> GetFirstNameSuggestions(
        GetFirstNameSuggestionsRequest request,
        ServerCallContext context)
    {
        var names = suggestionsQuery.Handle(request.FirstName.ToUpper());

        var response = new GetFirstNameSuggestionsResponse();

        response.FirstNameSuggestions.AddRange(names.Select(x => x.Capitalize()));

        return Task.FromResult(response);
    }
}