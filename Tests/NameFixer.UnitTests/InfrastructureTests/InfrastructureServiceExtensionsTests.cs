using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
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
}