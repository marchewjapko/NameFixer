using NameFixer.gRPCServices;

namespace NameFixer.IntegrationTests.WebApplicationFactory.Tests.ServiceTests;

[Parallelizable(ParallelScope.Self)]
public class LastNameServiceTests
{
    private readonly SuggestionsService.SuggestionsServiceClient _client;

    public LastNameServiceTests()
    {
        var fixture = new TestServerFixture();

        var channel = fixture.GrpcChannel;
        _client = new SuggestionsService.SuggestionsServiceClient(channel);
    }

    [Test]
    public async Task ShouldGetSuggestionsForFirstName()
    {
        // arrange
        var request = new GetSuggestionsRequest
        {
            Key = "Kowaski"
        };

        // act
        var result = await _client.GetLastNameSuggestionsAsync(request);

        // assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result.Suggestions, Has.Count.EqualTo(5));
                Assert.That(result.Suggestions, Does.Not.Contain("Kowaski"));
                Assert.That(result.Suggestions, Does.Contain("Kowalski"));
            });
    }
}