using Moq;
using NameFixer.Core.Entities;
using NameFixer.Core.Repositories;
using NameFixer.UseCases.Queries.Suggestions.GetSecondNameSuggestionsQuery;

namespace NameFixer.UnitTests.UseCasesTests.QueriesTests.SuggestionTests;

public class GetSecondNameSuggestionsQueryTests
{
    [Test]
    public void ShouldSuggestSecondNames()
    {
        //Arrange
        const string invalidName = "Johm";

        var entities = new List<SecondNameEntity>
        {
            new("John", 1),
            new("Jane", 1),
            new("Jon", 1),
            new("Johan", 1),
            new("Janet", 1),
            new("Chris", 1),
            new("Winston", 1)
        };

        var repositoryMock = new Mock<ISecondNameRepository>();
        repositoryMock
            .Setup(x => x.GetAll())
            .Returns(entities);

        var query = new GetSecondNameSuggestionsQuery(repositoryMock.Object);

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
                Assert.That(result, !Contains.Item("Chris"));
                Assert.That(result, !Contains.Item("Winston"));
                Assert.That(result, Contains.Item("John"));
            });
    }
}