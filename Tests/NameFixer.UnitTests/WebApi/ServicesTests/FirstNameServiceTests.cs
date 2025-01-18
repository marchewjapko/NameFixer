using Grpc.Core;
using Moq;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;
using FirstNameService = NameFixer.WebApi.Services.FirstNameService;

namespace NameFixer.UnitTests.WebApi.ServicesTests;

public class FirstNameServiceTests
{
    [Test]
    public async Task ShouldGetFirstNameSuggestions()
    {
        //Arrange
        const string firstName = "John";

        var context = new Mock<ServerCallContext>();

        var suggestionsQueryMock = new Mock<IGetFirstNameSuggestionsQuery>();
        suggestionsQueryMock
            .Setup(x => x.Handle(firstName.ToUpper()))
            .Returns(
                new List<string>
                {
                    "JOHN",
                    "JON",
                    "JOHAN",
                    "JANE",
                    "JONES"
                });

        var firstNameService = new FirstNameService(suggestionsQueryMock.Object);

        //Act
        var results = await firstNameService.GetFirstNameSuggestions(
            new GetFirstNameSuggestionsRequest
            {
                FirstName = firstName
            },
            context.Object);

        //Assert
        string[] expected =
        [
            "John", "Jon", "Johan", "Jane", "Jones"
        ];

        Assert.That(results.FirstNameSuggestions, Is.EquivalentTo(expected));
    }
}