using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NameFixer.Infrastructure.Repositories.Configurations;
using NameFixer.WebApi.Configurations;

namespace NameFixer.UnitTests.WebApi.ConfigurationsTests;

[TestFixture]
public class OptionsConfigurationTests
{
    [Test]
    public void AddOptionsConfigurations_ShouldConfigureOptions()
    {
        //Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            {
                "DatasetsOptions:FirstNameRepository:MinOccurrenceRate", "1000"
            },
            {
                "DatasetsOptions:FirstNameRepository:FemaleLocalPath", "SomePath1"
            },
            {
                "DatasetsOptions:FirstNameRepository:MaleLocalPath", "SomePath1"
            },
            {
                "DatasetsOptions:SecondNameRepository:MinOccurrenceRate", "1000"
            },
            {
                "DatasetsOptions:SecondNameRepository:FemaleLocalPath", "SomePath2"
            },
            {
                "DatasetsOptions:SecondNameRepository:MaleLocalPath", "SomePath2"
            },
            {
                "DatasetsOptions:LastNameRepository:MinOccurrenceRate", "1000"
            },
            {
                "DatasetsOptions:LastNameRepository:FemaleLocalPath", "SomePath3"
            },
            {
                "DatasetsOptions:LastNameRepository:MaleLocalPath", "SomePath3"
            }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        //Act
        var services = new ServiceCollection();
        services.AddOptionsConfigurations(configuration);

        //Assert
        var serviceProvider = services.BuildServiceProvider();
        var firstNameOptions =
            serviceProvider.GetService<IOptionsMonitor<RepositoryOptions>>()!.Get(RepositoryOptions.FirstName);

        var secondNameOptions =
            serviceProvider.GetService<IOptionsMonitor<RepositoryOptions>>()!.Get(RepositoryOptions.SecondName);

        var lastNameOptions =
            serviceProvider.GetService<IOptionsMonitor<RepositoryOptions>>()!.Get(RepositoryOptions.LastName);

        Assert.Multiple(
            () =>
            {
                Assert.That(firstNameOptions, Is.Not.Null);
                Assert.That(secondNameOptions, Is.Not.Null);
                Assert.That(lastNameOptions, Is.Not.Null);

                Assert.That(firstNameOptions.MinOccurrenceRate, Is.EqualTo(1000));
                Assert.That(secondNameOptions.MinOccurrenceRate, Is.EqualTo(1000));
                Assert.That(lastNameOptions.MinOccurrenceRate, Is.EqualTo(1000));

                Assert.That(firstNameOptions.FemaleLocalPath, Is.EqualTo("SomePath1"));
                Assert.That(secondNameOptions.FemaleLocalPath, Is.EqualTo("SomePath2"));
                Assert.That(lastNameOptions.FemaleLocalPath, Is.EqualTo("SomePath3"));
            });
    }

    [Test]
    public void ValidateOptions_ShouldThrowValidationException_WhenOptionsAreInvalid()
    {
        //Arrange
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>
        {
            {
                "DatasetsOptions:FirstNameRepository:MinOccurrenceRate", "1000"
            },
            {
                "DatasetsOptions:FirstNameRepository:FemaleLocalPath", null!
            },
            {
                "DatasetsOptions:FirstNameRepository:MaleLocalPath", "SomePath1"
            },
            {
                "DatasetsOptions:SecondNameRepository:MinOccurrenceRate", "1000"
            },
            {
                "DatasetsOptions:SecondNameRepository:FemaleLocalPath", "SomePath2"
            },
            {
                "DatasetsOptions:SecondNameRepository:MaleLocalPath", "SomePath2"
            },
            {
                "DatasetsOptions:LastNameRepository:MinOccurrenceRate", "1000"
            },
            {
                "DatasetsOptions:LastNameRepository:FemaleLocalPath", "SomePath3"
            },
            {
                "DatasetsOptions:LastNameRepository:MaleLocalPath", "SomePath3"
            }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        //Act
        var exception = Assert.Throws<ValidationException>(() => services.AddOptionsConfigurations(configuration));

        //Assert
        Assert.That(
            exception.Message,
            Is.EqualTo(
                "Options validation for section 'DatasetsOptions:FirstNameRepository' failed: The FemaleLocalPath field is required."));
    }

    [Test]
    public void ValidateOptions_ShouldThrowValidationException_WhenOptionsAreMalformed()
    {
        //Arrange
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>
        {
            {
                "DatasetsOptions:SomeProperty", "SomeValue"
            },
            {
                "DatasetsOptions:SecondName:SomeProperty", "SomeValue"
            },
            {
                "DatasetsOptions:LastName:SomeProperty", "SomeValue"
            }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        //Act
        var exception = Assert.Throws<ValidationException>(() => services.AddOptionsConfigurations(configuration));

        //Assert
        Assert.That(
            exception.Message,
            Is.EqualTo(
                "Unable to map configuration section DatasetsOptions:FirstNameRepository to NameFixer.Infrastructure.Repositories.Configurations.RepositoryOptions"));
    }
}