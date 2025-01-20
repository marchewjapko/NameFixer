using Grpc.Core;
using Moq;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Queries.Suggestions.GetFirstNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetLastNameSuggestionsQuery;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;
using NameFixer.WebApi.Services;

namespace NameFixer.UnitTests.WebApi.ServicesTests;

public class LastNameServiceTests
{
    [Test]
    public async Task ShouldGetLastNameSuggestions()
    {
        //Arrange
        const string lastName = "Montgomert";

        var context = new Mock<ServerCallContext>();

        var lastNameSuggestionsQueryMock = new Mock<IGetLastNameSuggestionsQuery>();
        lastNameSuggestionsQueryMock
            .Setup(x => x.Handle(lastName.ToUpper()))
            .Returns(
                new List<string>
                {
                    "MONTAGUE",
                    "MONTGOMERIE",
                    "MONTGOMER",
                    "MONTEMAYOR",
                    "MONTROSE"
                });

        var firstNameSuggestionsQueryMock = new Mock<IGetFirstNameSuggestionsQuery>();
        var secondNameSuggestionsQueryMock = new Mock<IGetSecondNameSuggestionsQuery>();

        var service = new SuggestionService(
            firstNameSuggestionsQueryMock.Object,
            secondNameSuggestionsQueryMock.Object,
            lastNameSuggestionsQueryMock.Object);

        //Act
        var results = await service.GetLastNameSuggestions(
            new GetSuggestionsRequest
            {
                Key = lastName
            },
            context.Object);

        //Assert
        string[] expected =
        [
            "Montague", "Montgomerie", "Montgomer", "Montemayor", "Montrose"
        ];

        Assert.That(results.Suggestions, Is.EquivalentTo(expected));
    }
}