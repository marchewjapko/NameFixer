using NameFixer.Core.Entities;

namespace NameFixer.UnitTests.CoreTests;

public class LastNameEntityTests
{
    [Test]
    public void ShouldNotEqual()
    {
        //Arrange
        var entity1 = new LastNameEntity("Clement", 1);
        var entity2 = new LastNameEntity("Stanley", 1);

        //Act
        var result = entity1.Equals(entity2);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void ShouldNotEqual_OtherIsNull()
    {
        //Arrange
        var entity1 = new LastNameEntity("Clement", 1);
        LastNameEntity? entity2 = null;

        //Act
        var result = entity1.Equals(entity2);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void ShouldEqual()
    {
        //Arrange
        var entity1 = new LastNameEntity("Clement", 1);
        var entity2 = new LastNameEntity("Clement", 1);

        //Act
        var result = entity1.Equals(entity2);

        //Assert
        Assert.That(result, Is.True);
    }
}