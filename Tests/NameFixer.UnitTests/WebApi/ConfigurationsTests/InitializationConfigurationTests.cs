using Moq;
using NameFixer.UseCases.Commands.InitializeFirstNamesCommand;
using NameFixer.UseCases.Commands.InitializeLastNamesCommand;
using NameFixer.UseCases.Commands.InitializeSecondNamesCommand;
using NameFixer.WebApi.Configurations;

namespace NameFixer.UnitTests.WebApi.ConfigurationsTests;

public class InitializationConfigurationTests
{
    [Test]
    public async Task ShouldInitializeStores()
    {
        //Arrange
        var firstNameCommand = new Mock<IInitializeFirstNamesCommand>();
        var secondNameCommand = new Mock<IInitializeSecondNamesCommand>();
        var lastNameCommand = new Mock<IInitializeLastNamesCommand>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IInitializeFirstNamesCommand)))
            .Returns(firstNameCommand.Object);

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IInitializeSecondNamesCommand)))
            .Returns(secondNameCommand.Object);

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IInitializeLastNamesCommand)))
            .Returns(lastNameCommand.Object);

        //Act
        await serviceProviderMock.Object.InitializeStores();

        //Assert
        firstNameCommand.Verify(x => x.Handle(), Times.Once);
        secondNameCommand.Verify(x => x.Handle(), Times.Once);
        lastNameCommand.Verify(x => x.Handle(), Times.Once);
    }

    [Test]
    public void ShouldNotInitializeStores_NoFirstNamesCommand()
    {
        //Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();

        //Act
        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await serviceProviderMock.Object.InitializeStores());

        //Assert
        Assert.That(exception, Has.Message.EqualTo("Service 'IInitializeFirstNamesCommand' was not found."));
    }

    [Test]
    public void ShouldNotInitializeStores_NoSecondNamesCommand()
    {
        //Arrange
        var firstNameCommand = new Mock<IInitializeFirstNamesCommand>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IInitializeFirstNamesCommand)))
            .Returns(firstNameCommand.Object);

        //Act
        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await serviceProviderMock.Object.InitializeStores());

        //Assert
        Assert.That(exception, Has.Message.EqualTo("Service 'IInitializeSecondNamesCommand' was not found."));
    }

    [Test]
    public void ShouldNotInitializeStores_NoLastNamesCommand()
    {
        //Arrange
        var firstNameCommand = new Mock<IInitializeFirstNamesCommand>();
        var secondNameCommand = new Mock<IInitializeSecondNamesCommand>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IInitializeFirstNamesCommand)))
            .Returns(firstNameCommand.Object);

        serviceProviderMock
            .Setup(x => x.GetService(typeof(IInitializeSecondNamesCommand)))
            .Returns(secondNameCommand.Object);

        //Act
        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await serviceProviderMock.Object.InitializeStores());

        //Assert
        Assert.That(exception, Has.Message.EqualTo("Service 'IInitializeLastNamesCommand' was not found."));
    }
}