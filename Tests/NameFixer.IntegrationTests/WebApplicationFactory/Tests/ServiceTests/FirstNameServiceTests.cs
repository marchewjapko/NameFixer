using NameFixer.gRPCServices;

namespace NameFixer.IntegrationTests.WebApplicationFactory.Tests.ServiceTests;

public class FirstNameServiceTests
{
    private readonly FirstNameService.FirstNameServiceClient _client;

    public FirstNameServiceTests()
    {
        var fixture = new TestServerFixture();

        var channel = fixture.GrpcChannel;
        _client = new FirstNameService.FirstNameServiceClient(channel);
    }

    [Test]
    public async Task ShouldGetSuggestionsForFirstName()
    {
        // arrange
        var request = new GetFirstNameSuggestionsRequest()
        {
            FirstName = "Lukasz"
        };

        // act
        var result = await _client.GetFirstNameSuggestionsAsync(request);

        // assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result.FirstNameSuggestions, Has.Count.EqualTo(5));
                Assert.That(result.FirstNameSuggestions, Does.Not.Contain("Lukasz"));
                Assert.That(result.FirstNameSuggestions, Does.Contain("Łukasz"));
            });
    }
}