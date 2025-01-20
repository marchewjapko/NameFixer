using Grpc.Core;
using Moq;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;
using NameFixer.WebApi.Services;

namespace NameFixer.UnitTests.WebApi.ServicesTests;

public class SecondNameServiceTests
{
    [Test]
    public async Task ShouldGetSecondNameSuggestions()
    {
        //Arrange
        const string secondName = "John";

        var context = new Mock<ServerCallContext>();

        var secondNameSuggestionsQueryMock = new Mock<IGetSecondNameSuggestionsQuery>();
        secondNameSuggestionsQueryMock
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

        var firstNameSuggestionsQueryMock = new Mock<IGetFirstNameSuggestionsQuery>();
        var lastNameSuggestionsQueryMock = new Mock<IGetLastNameSuggestionsQuery>();

        var service = new SuggestionService(
            firstNameSuggestionsQueryMock.Object,
            secondNameSuggestionsQueryMock.Object,
            lastNameSuggestionsQueryMock.Object);

        //Act
        var results = await service.GetSecondNameSuggestions(
            new GetSuggestionsRequest
            {
                Key = secondName
            },
            context.Object);

        //Assert
        string[] expected =
        [
            "John", "Jon", "Johan", "Jane", "Jones"
        ];

        Assert.That(results.Suggestions, Is.EquivalentTo(expected));
    }
}