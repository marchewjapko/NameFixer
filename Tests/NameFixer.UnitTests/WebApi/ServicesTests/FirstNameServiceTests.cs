using Grpc.Core;
using Moq;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;
using NameFixer.WebApi.Services;

namespace NameFixer.UnitTests.WebApi.ServicesTests;

public class FirstNameServiceTests
{
    [Test]
    public async Task ShouldGetFirstNameSuggestions()
    {
        //Arrange
        const string firstName = "John";

        var context = new Mock<ServerCallContext>();

        var firstNameSuggestionsQueryMock = new Mock<IGetFirstNameSuggestionsQuery>();
        firstNameSuggestionsQueryMock
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

        var secondNameSuggestionsQueryMock = new Mock<IGetSecondNameSuggestionsQuery>();
        var lastNameSuggestionsQueryMock = new Mock<IGetLastNameSuggestionsQuery>();

        var service = new SuggestionService(
            firstNameSuggestionsQueryMock.Object,
            secondNameSuggestionsQueryMock.Object,
            lastNameSuggestionsQueryMock.Object);

        //Act
        var results = await service.GetFirstNameSuggestions(
            new GetSuggestionsRequest
            {
                Key = firstName
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