using Moq;
using NameFixer.Core.Entities;
using NameFixer.Core.Repositories;
using NameFixer.UseCases.Queries.GetLastNameSuggestionsQuery;

namespace NameFixer.UnitTests.UseCasesTests.QueriesTests;

public class GetLastNameSuggestionsQueryTests
{
    [Test]
    public void ShouldSuggestSecondNames()
    {
        //Arrange
        const string invalidName = "McGe";

        var entities = new List<LastNameEntity>()
        {
            new("McGee", 1),
            new("McGeee", 1),
            new("McGea", 1),
            new("McGaee", 1),
            new("McGeaa", 1),
            new("Chamberlain", 1),
            new("Churchill", 1)
        };

        var repositoryMock = new Mock<ILastNameRepository>();
        repositoryMock
            .Setup(x => x.GetAll())
            .Returns(entities);

        var query = new GetLastNameSuggestionsQuery(repositoryMock.Object);

        //Act
        var result = query
            .Handle(invalidName)
            .ToList();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Has.Count.EqualTo(5));
                Assert.That(result, !Contains.Item(invalidName));
                Assert.That(result, !Contains.Item("Chamberlain"));
                Assert.That(result, !Contains.Item("Churchill"));
                Assert.That(result, Contains.Item("McGee"));
            });
    }
}