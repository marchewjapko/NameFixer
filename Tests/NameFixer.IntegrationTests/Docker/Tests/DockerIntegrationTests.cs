using Docker.DotNet;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Grpc.Net.Client;
using NameFixer.gRPCServices;

namespace NameFixer.IntegrationTests.Docker.Tests;

[Parallelizable(ParallelScope.Self)]
public class DockerIntegrationTests
{
    private readonly DockerClient _dockerClient = new DockerClientConfiguration().CreateClient();
    private IContainer? _container;
    private IImage? _image;

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _dockerClient.Dispose();
    }

    [Test]
    [Order(1)]
    public async Task ShouldBuildDockerImage()
    {
        //Arrange
        var containerName = $"name-fixer-integration-test:{Guid.NewGuid()}";

        var image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithName(containerName)
            .WithCleanUp(true)
            .Build();

        //Act
        await image.CreateAsync();

        // Assert
        var images = await _dockerClient.Images.ListImagesAsync(
            new ImagesListParameters
            {
                All = true
            });

        Assert.That(images.SelectMany(x => x.RepoTags), Contains.Item(containerName));

        _image = image;
    }

    [Test]
    [Order(2)]
    public async Task ShouldRunContainerAndBeHealthy()
    {
        //Arrange
        Assume.That(_image, Is.Not.Null);

        var container = new ContainerBuilder()
            .WithImage(_image)
            .WithPortBinding(8081, true)
            .WithPortBinding(8082, true)
            .WithEnvironment(
                new Dictionary<string, string>
                {
                    {
                        "Kestrel__Endpoints__Https__Url", "http://+:0"
                    }
                })
            .WithName("name-fixer-integration-test")
            .WithWaitStrategy(
                Wait
                    .ForUnixContainer()
                    .UntilContainerIsHealthy())
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .Build();

        //Act
        await container.StartAsync();

        //Assert
        Assert.That(container.Health, Is.EqualTo(TestcontainersHealthStatus.Healthy));

        _container = container;
    }

    [Test]
    [Order(3)]
    public async Task ShouldSuggestFirstNames()
    {
        //Arrange
        Assume.That(_container, Is.Not.Null);
        Assume.That(_container.Health, Is.EqualTo(TestcontainersHealthStatus.Healthy));

        var request = new GetSuggestionsRequest
        {
            Key = "Lukasz"
        };

        var uri = new UriBuilder
        {
            Scheme = "http",
            Host = _container.Hostname,
            Port = _container.GetMappedPublicPort(8081)
        }.Uri;

        var channel = GrpcChannel.ForAddress(uri);

        var client = new SuggestionsService.SuggestionsServiceClient(channel);

        //Act
        var result = await client.GetFirstNameSuggestionsAsync(request);

        //Assert
        Assert.Multiple(
            () =>
            {
                Assert.That(result.Suggestions, Has.Count.EqualTo(5));
                Assert.That(result.Suggestions, Does.Not.Contain("Lukasz"));
                Assert.That(result.Suggestions, Does.Contain("Łukasz"));
            });
    }
}