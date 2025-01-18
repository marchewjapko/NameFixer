using Bogus;
using NameFixer.Core.Entities;
using NameFixer.Core.Exceptions.CustomExceptions;
using NameFixer.Infrastructure.Repositories;

namespace NameFixer.UnitTests.InfrastructureTests.RepositoryTests;

public class FirstNameRepositoryTests
{
    private static readonly Faker Faker = new();

    [Test]
    public void ShouldGetAll()
    {
        //Arrange
        const int count = 100;

        var repository = new FirstNameRepository();

        var firstNames = Enumerable
            .Range(0, count)
            .Select(_ => new FirstNameEntity(Faker.Name.FirstName(), Faker.Random.Int(1, 100_000)));

        repository.AddRange(firstNames);

        //Act
        var result = repository.GetAll();

        //Assert
        Assert.That(result.Count, Is.EqualTo(count));
    }

    [Test]
    public void ShouldGetNotAll_NotInitializedException()
    {
        //Arrange
        var repository = new FirstNameRepository();

        //Act
        var exception = Assert.Throws<NotInitializedException>(() => repository.GetAll());

        //Assert
        Assert.That(
            exception,
            Has.Message.EqualTo("Repository NameFixer.Core.Entities.FirstNameEntity has not been initialized"));
    }
}