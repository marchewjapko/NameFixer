using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Moq;
using NameFixer.Core.Models.DatasetModels;
using NameFixer.Infrastructure.Services;

namespace NameFixer.UnitTests.InfrastructureTests.ServicesTests;

public class FileReaderTests
{
    private const string FileContents = """
                                        IMIĘ_PIERWSZE,PŁEĆ,LICZBA_WYSTĄPIEŃ
                                        JAN,MĘŻCZYZNA,1095287
                                        STANISŁAW,MĘŻCZYZNA,953493
                                        PIOTR,MĘŻCZYZNA,816910
                                        ANDRZEJ,MĘŻCZYZNA,744320
                                        KRZYSZTOF,MĘŻCZYZNA,736688
                                        """;

    [Test]
    public void ShouldReadCsvFile()
    {
        //Arrange
        const string fileName = "TestFile.csv";
        var fileSystemMock = new Mock<IFileSystem>();

        var fileSystem = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                [fileName] = new(FileContents)
            });

        fileSystemMock
            .Setup(x => x.File.OpenRead(fileName))
            .Returns(fileSystem.File.OpenRead(fileName));

        var fileReaderService = new FileReaderService(fileSystemMock.Object);

        //Act
        var result = fileReaderService
            .ReadCsvFile<FirstNameDatasetModel>(fileName)
            .ToList();

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Has.Count.EqualTo(5));
            });
    }
}