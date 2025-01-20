using NameFixer.gRPCServices;

namespace NameFixer.IntegrationTests.WebApplicationFactory.Tests.ServiceTests;

public class SecondNameServiceTests
{
    private readonly SecondNameService.SecondNameServiceClient _client;

    public SecondNameServiceTests()
    {
        var fixture = new TestServerFixture();

        var channel = fixture.GrpcChannel;
        _client = new SecondNameService.SecondNameServiceClient(channel);
    }

    [Test]
    public async Task ShouldGetSuggestionsForFirstName()
    {
        // arrange
        var request = new GetSecondNameSuggestionsRequest()
        {
            SecondName = "Kawol"
        };

        // act
        var result = await _client.GetSecondNameSuggestionsAsync(request);

        // assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result.SecondNameSuggestions, Has.Count.EqualTo(5));
                Assert.That(result.SecondNameSuggestions, Does.Not.Contain("Kawol"));
                Assert.That(result.SecondNameSuggestions, Does.Contain("Karol"));
            });
    }
}