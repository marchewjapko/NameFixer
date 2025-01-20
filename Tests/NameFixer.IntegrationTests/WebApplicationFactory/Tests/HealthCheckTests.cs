namespace NameFixer.IntegrationTests.WebApplicationFactory.Tests;

[Parallelizable(ParallelScope.Self)]
public class HealthCheckTests
{
    private readonly HttpClient _client;

    public HealthCheckTests()
    {
        var fixture = new TestServerFixture();
        _client = fixture.HttpClient;
    }

    [Test]
    public async Task ShouldGetSuggestionsForFirstName()
    {
        // arrange
        const string pathToHealthChecks = "/_health";

        // act
        var result = await _client.GetAsync(pathToHealthChecks);

        // assert
        result.EnsureSuccessStatusCode();
    }
}