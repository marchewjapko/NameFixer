using Google.Rpc;
using Grpc.Core;
using NameFixer.Core.Exceptions.CustomExceptions;

namespace NameFixer.UnitTests.CoreTests;

public class NotInitializedExceptionTests
{
    [Test]
    public void ShouldMapToToRpcException()
    {
        //Arrange
        const string repositoryName = "ISomeRepository";

        var exception = new NotInitializedException(repositoryName);

        //Act
        var result = exception.ToRpcException();

        //Assert
        var errorInfoDetails = result
            .GetRpcStatus()
            ?.GetDetail<ErrorInfo>();

        Assert.Multiple(
            () =>
            {
                Assert.That(result, Is.TypeOf<RpcException>());
                Assert.That(result.Status.StatusCode, Is.EqualTo(StatusCode.Internal));
                Assert.That(errorInfoDetails, Is.Not.Null);
                Assert.That(errorInfoDetails!.Domain, Is.EqualTo("NameFixer.WebApi"));
                Assert.That(errorInfoDetails.Reason, Is.EqualTo("REPOSITORY_NOT_INITIALIZED"));
                Assert.That(errorInfoDetails.Metadata, Is.Not.Empty);
                Assert.That(
                    errorInfoDetails.Metadata.First()
                        .Key,
                    Is.EqualTo("repositoryName"));

                Assert.That(
                    errorInfoDetails.Metadata.First()
                        .Value,
                    Is.EqualTo(repositoryName));
            });
    }
}