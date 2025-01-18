using Microsoft.Extensions.DependencyInjection;
using NameFixer.UseCases;

namespace NameFixer.UnitTests.UseCasesTests;

public class UseCasesServiceExtensionsTests
{
    [Test]
    public void ShouldRegister()
    {
        //Arrange
        var services = new ServiceCollection();
        var initialServicesCount = services.Count;

        //Act
        services.AddUseCasesServices();

        //Assert
        Assert.That(services, Has.Count.EqualTo(initialServicesCount + 9));
    }
}