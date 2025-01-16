using System.Net;

namespace NameFixer.UnitTests.InfrastructureTests.ServicesTests;

public class MockHttpClientHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Now that's come content.")
            });
    }
}