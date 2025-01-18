using Grpc.Core;

namespace NameFixer.Core.Exceptions;

public interface ICustomMappedException
{
    public RpcException ToRpcException();
}