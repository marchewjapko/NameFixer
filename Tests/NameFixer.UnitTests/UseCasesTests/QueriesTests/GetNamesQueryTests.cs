using System.IO.Abstractions;
using Bogus;
using Moq;
using NameFixer.Core.Models.DatasetModels;
using NameFixer.Core.ServicesInterfaces;
using NameFixer.UseCases.Queries.GetNamesQuery;

namespace NameFixer.UnitTests.UseCasesTests.QueriesTests;

public class GetNamesQueryTests
{
    [Test]
    public async Task ShouldGetNames_Locally()
    {
        //Arrange
        var writerMock = new Mock<IFileWriterService>();
        var readerMock = new Mock<IFileReaderService>();
        var fileSystemMock = new Mock<IFileSystem>();

        const int count = 100;
        const string localPath = "LocalPath";

        var firstNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.NumberOfOccurrences, f => f.Random.Int(1, 100_000))
            .Generate(count);

        fileSystemMock
            .Setup(x => x.File.Exists(localPath))
            .Returns(true);

        readerMock
            .Setup(x => x.ReadCsvFile<FirstNameDatasetModel>(localPath))
            .Returns(firstNames);

        var query = new GetNamesQuery<FirstNameDatasetModel>(
            writerMock.Object,
            readerMock.Object,
            fileSystemMock.Object);

        //Act
        var result = await query.Handle(localPath);

        result = result.ToList();

        //Assert
        Assert.That(result.Count(), Is.EqualTo(count));

        fileSystemMock.Verify(x => x.File.Exists(localPath), Times.Once);

        readerMock.Verify(x => x.ReadCsvFile<FirstNameDatasetModel>(localPath), Times.Once);
    }

    [Test]
    public async Task ShouldGetNames_Remote()
    {
        //Arrange
        var writerMock = new Mock<IFileWriterService>();
        var readerMock = new Mock<IFileReaderService>();
        var fileSystemMock = new Mock<IFileSystem>();

        const int count = 100;
        const string localPath = "LocalPath";
        const string remotePath = "RemotePath";

        var firstNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.NumberOfOccurrences, f => f.Random.Int(1, 100_000))
            .Generate(count);

        fileSystemMock
            .Setup(x => x.File.Exists(localPath))
            .Returns(false);

        readerMock
            .Setup(x => x.ReadCsvFile<FirstNameDatasetModel>(localPath))
            .Returns(firstNames);

        var query = new GetNamesQuery<FirstNameDatasetModel>(
            writerMock.Object,
            readerMock.Object,
            fileSystemMock.Object);

        //Act
        var result = await query.Handle(localPath, remotePath);

        result = result.ToList();

        //Assert
        Assert.That(result.Count(), Is.EqualTo(count));

        fileSystemMock.Verify(x => x.File.Exists(localPath), Times.Once);

        writerMock.Verify(x => x.WriteToFile(remotePath, localPath), Times.Once);

        readerMock.Verify(x => x.ReadCsvFile<FirstNameDatasetModel>(localPath), Times.Once);
    }

    [Test]
    public void ShouldNotGetNames_LocalPathDoesNotExistRemoteNotProvided()
    {
        //Arrange
        var writerMock = new Mock<IFileWriterService>();
        var readerMock = new Mock<IFileReaderService>();
        var fileSystemMock = new Mock<IFileSystem>();

        const string localPath = "LocalPath";

        fileSystemMock
            .Setup(x => x.File.Exists(localPath))
            .Returns(false);

        var query = new GetNamesQuery<FirstNameDatasetModel>(
            writerMock.Object,
            readerMock.Object,
            fileSystemMock.Object);

        //Act
        var exception = Assert.ThrowsAsync<ArgumentException>(async () => await query.Handle(localPath));

        //Assert
        Assert.That(
            exception,
            Has.Message.EqualTo("Path to file 'LocalPath' has not been found, remote path has not been provided."));
    }
    //
    // [Test]
    // public void ShouldNotGetNames_ErrorDuringDownload()
    // {
    //     //Arrange
    //     var writerMock = new Mock<IFileWriterService>();
    //     var readerMock = new Mock<IFileReaderService>();
    //     var fileSystemMock = new Mock<IFileSystem>();
    //
    //     const string localPath = "LocalPath";
    //     const string remotePath = "RemotePath";
    //
    //     fileSystemMock
    //         .Setup(x => x.File.Exists(localPath))
    //         .Returns(false);
    //
    //     writerMock
    //         .Setup(x => x.WriteToFile(remotePath, localPath))
    //         .Throws(new Exception("This is an inner exception"));
    //
    //     var query = new GetNamesQuery<FirstNameDatasetModel>(
    //         writerMock.Object,
    //         readerMock.Object,
    //         fileSystemMock.Object);
    //
    //     //Act
    //     var exception =
    //         Assert.ThrowsAsync<DatasetDownloadProcessFailedException>(
    //             async () => await query.Handle(localPath, remotePath));
    //
    //     //Assert
    //     Assert.That(exception, Has.Message.EqualTo("Downloading file from 'RemotePath' failed."));
    //     Assert.That(exception.InnerException, Has.Message.EqualTo("This is an inner exception"));
    // }
}