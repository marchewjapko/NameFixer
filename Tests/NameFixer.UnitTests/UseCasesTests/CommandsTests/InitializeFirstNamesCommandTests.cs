using Bogus;
using Bogus.DataSets;
using Microsoft.Extensions.Options;
using Moq;
using NameFixer.Core.Entities;
using NameFixer.Core.Models.DatasetModels;
using NameFixer.Core.Repositories;
using NameFixer.UseCases.Commands.InitializeFirstNamesCommand;
using NameFixer.UseCases.Options;
using NameFixer.UseCases.Queries.GetNamesQuery;

namespace NameFixer.UnitTests.UseCasesTests.CommandsTests;

public class InitializeFirstNamesCommandTests
{
    [Test]
    public async Task ShouldInitialize()
    {
        //Arrange
        var repositoryMock = new Mock<IFirstNameRepository>();
        var optionsMock = new Mock<IOptionsMonitor<RepositoryOptions>>();
        var getNamesQueryMock = new Mock<IGetNamesQuery<FirstNameDatasetModel>>();

        var options = new RepositoryOptions
        {
            MinOccurrenceRate = 100,
            MaleLocalPath = "Male",
            FemaleLocalPath = "Female"
        };

        optionsMock
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(options);

        var femaleNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName(Name.Gender.Female))
            .RuleFor(x => x.NumberOfOccurrences, f => f.Random.Int(1, 100_000))
            .Generate(100);

        var maleNames = new Faker<FirstNameDatasetModel>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName(Name.Gender.Male))
            .RuleFor(x => x.NumberOfOccurrences, f => f.Random.Int(1, 100_000))
            .Generate(100);

        getNamesQueryMock
            .Setup(x => x.Handle(options.FemaleLocalPath, null))
            .ReturnsAsync(femaleNames);

        getNamesQueryMock
            .Setup(x => x.Handle(options.MaleLocalPath, null))
            .ReturnsAsync(maleNames);

        var command = new InitializeFirstNamesCommand(
            repositoryMock.Object,
            optionsMock.Object,
            getNamesQueryMock.Object);

        //Act
        await command.Handle();

        //Assert
        repositoryMock.Verify(x => x.AddRange(It.IsAny<IEnumerable<FirstNameEntity>>()), Times.Exactly(2));
    }
}