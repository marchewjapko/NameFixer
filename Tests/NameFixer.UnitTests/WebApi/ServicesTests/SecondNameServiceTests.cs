using Grpc.Core;
using Moq;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;
using SecondNameService = NameFixer.WebApi.Services.SecondNameService;

namespace NameFixer.UnitTests.WebApi.ServicesTests;

public class SecondNameServiceTests
{
    [Test]
    public async Task ShouldGetSecondNameSuggestions()
    {
        //Arrange
        const string secondName = "John";

        var context = new Mock<ServerCallContext>();

        var suggestionsQueryMock = new Mock<IGetSecondNameSuggestionsQuery>();
        suggestionsQueryMock
            .Setup(x => x.Handle(secondName.ToUpper()))
            .Returns(
                new List<string>
                {
                    "JOHN",
                    "JON",
                    "JOHAN",
                    "JANE",
                    "JONES"
                });

        var firstNameService = new SecondNameService(suggestionsQueryMock.Object);

        //Act
        var results = await firstNameService.GetSecondNameSuggestions(
            new GetSecondNameSuggestionsRequest
            {
                SecondName = secondName
            },
            context.Object);

        //Assert
        string[] expected =
        [
            "John", "Jon", "Johan", "Jane", "Jones"
        ];

        Assert.That(results.SecondNameSuggestions, Is.EquivalentTo(expected));
    }
}