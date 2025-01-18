using Bogus;
using NameFixer.Core.Entities;
using NameFixer.Core.Exceptions.CustomExceptions;
using NameFixer.Infrastructure.Repositories;

namespace NameFixer.UnitTests.InfrastructureTests.RepositoryTests;

public class SecondNameRepositoryTests
{
    private static readonly Faker Faker = new();

    [Test]
    public void ShouldGetAll()
    {
        //Arrange
        const int count = 100;

        var repository = new SecondNameRepository();

        var secondNames = Enumerable
            .Range(0, count)
            .Select(_ => new SecondNameEntity(Faker.Name.FirstName(), Faker.Random.Int(1, 100_000)));

        repository.AddRange(secondNames);

        //Act
        var result = repository.GetAll();

        //Assert
        Assert.That(result.Count, Is.EqualTo(count));
    }

    [Test]
    public void ShouldGetNotAll_NotInitializedException()
    {
        //Arrange
        var repository = new SecondNameRepository();

        //Act
        var exception = Assert.Throws<NotInitializedException>(() => repository.GetAll());

        //Assert
        Assert.That(
            exception,
            Has.Message.EqualTo("Repository NameFixer.Core.Entities.SecondNameEntity has not been initialized"));
    }
}