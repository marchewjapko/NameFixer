using NameFixer.gRPCServices;

namespace NameFixer.IntegrationTests.WebApplicationFactory.Tests.ServiceTests;

public class LastNameServiceTests
{
    private readonly LastNameService.LastNameServiceClient _client;

    public LastNameServiceTests()
    {
        var fixture = new TestServerFixture();

        var channel = fixture.GrpcChannel;
        _client = new LastNameService.LastNameServiceClient(channel);
    }

    [Test]
    public async Task ShouldGetSuggestionsForFirstName()
    {
        // arrange
        var request = new GetLastNameSuggestionsRequest()
        {
            LastName = "Kowaski"
        };

        // act
        var result = await _client.GetLastNameSuggestionsAsync(request);

        // assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result.LastNameSuggestions, Has.Count.EqualTo(5));
                Assert.That(result.LastNameSuggestions, Does.Not.Contain("Kowaski"));
                Assert.That(result.LastNameSuggestions, Does.Contain("Kowalski"));
            });
    }
}