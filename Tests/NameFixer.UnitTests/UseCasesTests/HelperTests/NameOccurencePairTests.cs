using Bogus;
using NameFixer.UseCases.Helpers;

namespace NameFixer.UnitTests.UseCasesTests.HelperTests;

public class NameOccurencePairTests
{
    private static readonly Faker Faker = new();

    [Test]
    public void XIsLargerWhenYIsNull()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        var x = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 2
        };

        NameOccurencePair y = null!;

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void YIsLargerWhenXIsNull()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        NameOccurencePair x = null!;

        var y = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 2
        };

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.LessThan(0));
    }

    [Test]
    public void XEqualsYWhenBothAreNull()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        NameOccurencePair x = null!;

        NameOccurencePair y = null!;

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void XIsLargerWhenDistanceIsZero()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        var x = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 0
        };

        var y = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 2
        };

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void YIsLargerWhenDistanceIsZero()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        var x = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 2
        };

        var y = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 0
        };

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.LessThan(0));
    }

    [Test]
    public void XIsLargerWhenDistanceIsEqualButOccurenceIsGreater()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        var x = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100_000,
            LevenshteinDistance = 2
        };

        var y = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 2
        };

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void XIsGreateWhenDistanceIsSmaller()
    {
        //Arrange
        var comparer = new NameOccurencePairComparer();

        var x = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100,
            LevenshteinDistance = 2
        };

        var y = new NameOccurencePair()
        {
            Name = Faker.Name.FirstName(),
            Occurence = 100_000,
            LevenshteinDistance = 6
        };

        //Act
        var result = comparer.Compare(x, y);

        //Assert
        Assert.That(result, Is.GreaterThan(0));
    }
}