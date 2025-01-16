using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Moq;
using NameFixer.Infrastructure.Services;

namespace NameFixer.UnitTests.InfrastructureTests.ServicesTests;

public class FileWriterTests
{
    [Test]
    public async Task ShouldWriteToFile()
    {
        //Arrange
        const string fileName = "TestFile.csv";
        var fileSystemMock = new Mock<IFileSystem>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        var fileSystem = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                [fileName] = new("")
            });

        fileSystemMock
            .Setup(x => x.File.Create(fileName))
            .Returns(fileSystem.File.Create(fileName));

        httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient(new MockHttpClientHandler()));

        var fileWriter = new FileWriterService(fileSystemMock.Object, httpClientFactoryMock.Object);

        //Act
        await fileWriter.WriteToFile("https://127.0.0.1:100/GoAway", fileName);

        //Assert
        fileSystemMock.Verify(x => x.File.Create(fileName), Times.Once);
    }
}