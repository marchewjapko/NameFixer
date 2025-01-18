using NameFixer.UseCases.Helpers;

namespace NameFixer.UnitTests.UseCasesTests.HelperTests;

public class StringExtensionTests
{
    [Test]
    public void ShouldCapitalize()
    {
        //Arrange
        const string input = "ANTHONY";

        //Act
        var result = input.Capitalize();

        //Assert
        Assert.That(result, Is.EqualTo("Anthony"));
    }
}