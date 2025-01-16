using System.IO.Abstractions;
using Bogus;
using Bogus.DataSets;
using Microsoft.Extensions.Options;
using Moq;
using NameFixer.Core.Exceptions;
using NameFixer.Core.ServicesInterfaces;
using NameFixer.Infrastructure.Models.DatasetFileModels;
using NameFixer.Infrastructure.Repositories;
using NameFixer.Infrastructure.Repositories.Configurations;

namespace NameFixer.UnitTests.InfrastructureTests.RepositoryTests;

public class FirstNameRepositoryTests
{
    private const int MinOccurrenceRate = 1_000;
    private static readonly Faker Faker = new();

    private static readonly Mock<IOptionsMonitor<RepositoryOptions>> OptionsMock = new();
    private static readonly Mock<IFileReaderService> FileReaderMock = new();
    private static readonly Mock<IFileWriterService> FileWriterMock = new();
    private static readonly Mock<IFileSystem> FileSystemMock = new();

    private static FirstNameRepository _repository = null!;

    public FirstNameRepositoryTests()
    {
        OptionsMock
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(
                new RepositoryOptions
                {
                    MinOccurrenceRate = MinOccurrenceRate,
                    FemaleLocalPath = Faker.System.FilePath(),
                    MaleLocalPath = Faker.System.FilePath(),
                    FemaleRemotePath = Faker.Internet.Url(),
                    MaleRemotePath = Faker.Internet.Url()
                });
    }

    [SetUp]
    public void Setup()
    {
        _repository = new FirstNameRepository(
            OptionsMock.Object,
            FileReaderMock.Object,
            FileWriterMock.Object,
            FileSystemMock.Object);
    }

    [Test]
    public async Task ShouldInitialize_ShouldReadFromLocalFile()
    {
        //Arrange
        var femaleNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName(Name.Gender.Female))
            .RuleFor(x => x.NumberOfOccurrences, f => f.IndexFaker)
            .Generate(1000);

        var maleNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName(Name.Gender.Male))
            .RuleFor(x => x.NumberOfOccurrences, f => f.IndexFaker + femaleNames.Count)
            .Generate(1000);

        FileSystemMock
            .Setup(x => x.File.Exists(It.IsAny<string>()))
            .Returns(true);

        FileReaderMock
            .Setup(
                x => x.ReadCsvFile<FirstNameDatasetModel>(
                    OptionsMock.Object.Get(It.IsAny<string>())
                        .FemaleLocalPath))
            .Returns(femaleNames);

        FileReaderMock
            .Setup(
                x => x.ReadCsvFile<FirstNameDatasetModel>(
                    OptionsMock.Object.Get(It.IsAny<string>())
                        .MaleLocalPath))
            .Returns(maleNames);

        //Act
        await _repository.Initialize();

        //Assert
        var numberOfExpected = femaleNames.Count(x => x.NumberOfOccurrences >= MinOccurrenceRate) +
                               maleNames.Count(x => x.NumberOfOccurrences >= MinOccurrenceRate);

        Assert.That(
            _repository
                .GetAll()
                .Count(),
            Is.EqualTo(numberOfExpected));
    }

    [Test]
    public async Task ShouldInitialize_ShouldReadFromRemoteFile()
    {
        //Arrange
        var femaleNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName(Name.Gender.Female))
            .RuleFor(x => x.NumberOfOccurrences, f => f.IndexFaker)
            .Generate(1000);

        var maleNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName(Name.Gender.Male))
            .RuleFor(x => x.NumberOfOccurrences, f => f.IndexFaker + femaleNames.Count)
            .Generate(1000);

        FileSystemMock
            .Setup(x => x.File.Exists(It.IsAny<string>()))
            .Returns(false);

        FileReaderMock
            .Setup(
                x => x.ReadCsvFile<FirstNameDatasetModel>(
                    OptionsMock.Object.Get(It.IsAny<string>())
                        .FemaleLocalPath))
            .Returns(femaleNames);

        FileReaderMock
            .Setup(
                x => x.ReadCsvFile<FirstNameDatasetModel>(
                    OptionsMock.Object.Get(It.IsAny<string>())
                        .MaleLocalPath))
            .Returns(maleNames);

        //Act
        await _repository.Initialize();

        //Assert
        var numberOfExpected = femaleNames.Count(x => x.NumberOfOccurrences >= MinOccurrenceRate) +
                               maleNames.Count(x => x.NumberOfOccurrences >= MinOccurrenceRate);

        Assert.That(
            _repository
                .GetAll()
                .Count(),
            Is.EqualTo(numberOfExpected));
    }

    [Test]
    public void ShouldNotInitialize_ShouldThrowLocalPathNotFoundRemotePathNotProvidedException()
    {
        //Arrange
        FileSystemMock
            .Setup(x => x.File.Exists(It.IsAny<string>()))
            .Returns(false);

        OptionsMock
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(
                new RepositoryOptions
                {
                    MinOccurrenceRate = MinOccurrenceRate,
                    FemaleLocalPath = Faker.System.FilePath(),
                    FemaleRemotePath = Faker.Internet.Url(),
                    MaleLocalPath = Faker.System.FilePath(),
                    MaleRemotePath = null
                });

        _repository = new FirstNameRepository(
            OptionsMock.Object,
            FileReaderMock.Object,
            FileWriterMock.Object,
            FileSystemMock.Object);

        //Act
        var exception =
            Assert.ThrowsAsync<LocalPathNotFoundRemotePathNotProvidedException>(
                async () => await _repository.Initialize());

        //Assert
        Assert.That(
            exception,
            Has.Message.EqualTo(
                $"Path to file '{OptionsMock.Object.Get(It.IsAny<string>()).MaleLocalPath}' has not been found, remote path has not been provided."));
    }

    [Test]
    public void ShouldNotInitialize_ShouldThrowDatasetDownloadProcessFailedException()
    {
        //Arrange
        FileSystemMock
            .Setup(x => x.File.Exists(It.IsAny<string>()))
            .Returns(false);

        FileWriterMock
            .Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("This is an inner exception"));

        _repository = new FirstNameRepository(
            OptionsMock.Object,
            FileReaderMock.Object,
            FileWriterMock.Object,
            FileSystemMock.Object);

        //Act
        var exception =
            Assert.ThrowsAsync<DatasetDownloadProcessFailedException>(async () => await _repository.Initialize());

        //Assert
        Assert.That(
            exception,
            Has.Message.EqualTo(
                $"Downloading file from '{OptionsMock.Object.Get(It.IsAny<string>()).MaleRemotePath!}' failed."));
    }
}