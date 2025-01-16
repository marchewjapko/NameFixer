using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NameFixer.Core.Exceptions;
using NameFixer.Core.Repositories;
using NameFixer.Core.ServicesInterfaces;
using NameFixer.Infrastructure;
using NameFixer.Infrastructure.Repositories;
using NameFixer.Infrastructure.Services;

namespace NameFixer.UnitTests.InfrastructureTests;

public class InfrastructureServiceExtensionsTests
{
    [Test]
    public void ShouldRegister()
    {
        //Arrange
        var services = new ServiceCollection();

        //Act
        services.AddInfrastructureServices();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(
                    services.Any(
                        x => x.ServiceType == typeof(IFirstNameRepository) &&
                             x.ImplementationType == typeof(FirstNameRepository)),
                    Is.True);

                Assert.That(
                    services.Any(
                        x => x.ServiceType == typeof(ISecondNameRepository) &&
                             x.ImplementationType == typeof(SecondNameRepository)),
                    Is.True);

                Assert.That(
                    services.Any(
                        x => x.ServiceType == typeof(ILastNameRepository) &&
                             x.ImplementationType == typeof(LastNameRepository)),
                    Is.True);

                Assert.That(
                    services.Any(
                        x => x.ServiceType == typeof(IFileReaderService) &&
                             x.ImplementationType == typeof(FileReaderService)),
                    Is.True);

                Assert.That(
                    services.Any(
                        x => x.ServiceType == typeof(IFileWriterService) &&
                             x.ImplementationType == typeof(FileWriterService)),
                    Is.True);

                Assert.That(
                    services.Any(
                        x => x.ServiceType == typeof(IFileSystem) && x.ImplementationType == typeof(FileSystem)),
                    Is.True);

                Assert.That(services.Any(x => x.ServiceType == typeof(HttpClient)), Is.True);
            });
    }

    [Test]
    public async Task ShouldInitializeRepositories()
    {
        //Arrange
        var servicesMock = new Mock<IServiceProvider>();

        var firstNameRepositoryMock = new Mock<IFirstNameRepository>();
        var secondNameRepositoryMock = new Mock<ISecondNameRepository>();
        var lastNameRepositoryMock = new Mock<ILastNameRepository>();

        servicesMock
            .Setup(x => x.GetService(typeof(IFirstNameRepository)))
            .Returns(firstNameRepositoryMock.Object);

        servicesMock
            .Setup(x => x.GetService(typeof(ISecondNameRepository)))
            .Returns(secondNameRepositoryMock.Object);

        servicesMock
            .Setup(x => x.GetService(typeof(ILastNameRepository)))
            .Returns(lastNameRepositoryMock.Object);

        //Act
        await servicesMock.Object.InitializeRepositories();

        //Assert
        firstNameRepositoryMock.Verify(x => x.Initialize(), Times.Once);
        secondNameRepositoryMock.Verify(x => x.Initialize(), Times.Once);
        lastNameRepositoryMock.Verify(x => x.Initialize(), Times.Once);
    }

    [Test]
    public void ShouldNotInitializeRepositories_NoFirstNameRepository()
    {
        //Arrange
        var servicesMock = new Mock<IServiceProvider>();

        servicesMock
            .Setup(x => x.GetService(typeof(IFirstNameRepository)))
            .Returns(null!);

        //Act
        var exception =
            Assert.ThrowsAsync<ServiceNotFoundException>(
                async () => await servicesMock.Object.InitializeRepositories());


        //Assert
        Assert.That(exception.Message, Is.EqualTo("Service IFirstNameRepository was not found."));
    }

    [Test]
    public void ShouldNotInitializeRepositories_NoSecondNameRepository()
    {
        //Arrange
        var servicesMock = new Mock<IServiceProvider>();

        var firstNameRepositoryMock = new Mock<IFirstNameRepository>();

        servicesMock
            .Setup(x => x.GetService(typeof(IFirstNameRepository)))
            .Returns(firstNameRepositoryMock.Object);

        servicesMock
            .Setup(x => x.GetService(typeof(ISecondNameRepository)))
            .Returns(null!);

        //Act
        var exception =
            Assert.ThrowsAsync<ServiceNotFoundException>(
                async () => await servicesMock.Object.InitializeRepositories());


        //Assert
        firstNameRepositoryMock.Verify(x => x.Initialize(), Times.Once);
        Assert.That(exception.Message, Is.EqualTo("Service ISecondNameRepository was not found."));
    }

    [Test]
    public void ShouldNotInitializeRepositories_NoLastNameRepository()
    {
        //Arrange
        var servicesMock = new Mock<IServiceProvider>();

        var firstNameRepositoryMock = new Mock<IFirstNameRepository>();
        var secondNameRepositoryMock = new Mock<ISecondNameRepository>();

        servicesMock
            .Setup(x => x.GetService(typeof(IFirstNameRepository)))
            .Returns(firstNameRepositoryMock.Object);

        servicesMock
            .Setup(x => x.GetService(typeof(ISecondNameRepository)))
            .Returns(secondNameRepositoryMock.Object);

        servicesMock
            .Setup(x => x.GetService(typeof(ILastNameRepository)))
            .Returns(null!);

        //Act
        var exception =
            Assert.ThrowsAsync<ServiceNotFoundException>(
                async () => await servicesMock.Object.InitializeRepositories());


        //Assert
        firstNameRepositoryMock.Verify(x => x.Initialize(), Times.Once);
        secondNameRepositoryMock.Verify(x => x.Initialize(), Times.Once);
        Assert.That(exception.Message, Is.EqualTo("Service ILastNameRepository was not found."));
    }
}