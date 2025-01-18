using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Status = Google.Rpc.Status;

namespace NameFixer.Core.Exceptions.CustomExceptions;

public class NotInitializedException(string repository)
    : Exception($"Repository {repository} has not been initialized"), ICustomMappedException
{
    public RpcException ToRpcException()
    {
        return new Status
        {
            Code = (int)Code.Internal,
            Message = $"Repository '{repository}'not initialized.",
            Details =
            {
                Any.Pack(
                    new ErrorInfo
                    {
                        Domain = "NameFixer.WebApi",
                        Reason = "REPOSITORY_NOT_INITIALIZED",
                        Metadata =
                        {
                            {
                                "repositoryName", repository
                            }
                        }
                    })
            }
        }.ToRpcException();
    }
}