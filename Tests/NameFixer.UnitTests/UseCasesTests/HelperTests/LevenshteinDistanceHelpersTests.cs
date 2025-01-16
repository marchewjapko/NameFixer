using NameFixer.UseCases.Helpers;

namespace NameFixer.UnitTests.UseCasesTests.HelperTests;

public class LevenshteinDistanceHelpersTests
{
    [Test]
    [TestCase("John", "Johm", 1)]
    [TestCase("MacDonald", "McDonalt", 2)]
    [TestCase("Macmillan", "Mcmilam", 3)]
    public void Calculate_ShouldCalculate(string input1, string input2, int distance)
    {
        //Act
        var result = LevenshteinDistance.Calculate(input1, input2);

        //Assert
        Assert.That(result, Is.EqualTo(distance));
    }

    [Test]
    public void Calculate_ShouldCalculate_Input1_IsEmpty()
    {
        //Act
        const string input1 = "";
        const string input2 = "John";

        var result = LevenshteinDistance.Calculate(input1, input2);

        //Assert
        Assert.That(result, Is.EqualTo(input2.Length));
    }

    [Test]
    public void Calculate_ShouldCalculate_Input2_IsEmpty()
    {
        //Act
        const string input1 = "John";
        const string input2 = "";

        var result = LevenshteinDistance.Calculate(input1, input2);

        //Assert
        Assert.That(result, Is.EqualTo(input1.Length));
    }
}