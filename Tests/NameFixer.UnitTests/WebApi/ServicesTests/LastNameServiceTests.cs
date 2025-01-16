using Grpc.Core;
using Moq;
using NameFixer.gRPCServices;
using NameFixer.UseCases.Queries.GetLastNameSuggestionsQuery;
using LastNameService = NameFixer.WebApi.Services.LastNameService;

namespace NameFixer.UnitTests.WebApi.ServicesTests;

public class LastNameServiceTests
{
    [Test]
    public async Task ShouldGetLastNameSuggestions()
    {
        //Arrange
        const string lastName = "Montgomert";

        var context = new Mock<ServerCallContext>();

        var suggestionsQueryMock = new Mock<IGetLastNameSuggestionsQuery>();
        suggestionsQueryMock
            .Setup(x => x.Handle(lastName.ToUpper()))
            .Returns(
                new List<string>()
                {
                    "MONTAGUE",
                    "MONTGOMERIE",
                    "MONTGOMER",
                    "MONTEMAYOR",
                    "MONTROSE"
                });

        var firstNameService = new LastNameService(suggestionsQueryMock.Object);

        //Act
        var results = await firstNameService.GetLastNameSuggestions(
            new GetLastNameSuggestionsRequest
            {
                LastName = lastName
            },
            context.Object);

        //Assert
        string[] expected =
        [
            "Montague", "Montgomerie", "Montgomer", "Montemayor", "Montrose"
        ];

        Assert.That(results.LastNameSuggestions, Is.EquivalentTo(expected));
    }
}