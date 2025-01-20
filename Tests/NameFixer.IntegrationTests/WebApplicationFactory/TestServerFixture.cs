using Grpc.Net.Client;

namespace NameFixer.IntegrationTests.WebApplicationFactory;

public sealed class TestServerFixture : IDisposable
{
    private readonly CustomWebApplicationFactory _factory = new();

    public TestServerFixture()
    {
        var client = _factory.CreateDefaultClient(new ResponseVersionHandler());

        HttpClient = client;

        var channelOptions = new GrpcChannelOptions
        {
            HttpClient = client
        };

        GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress!, channelOptions);
    }

    public GrpcChannel GrpcChannel { get; }

    public HttpClient HttpClient { get; }

    public void Dispose()
    {
        _factory.Dispose();
    }

    private class ResponseVersionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.Version = request.Version;
            return response;
        }
    }
}