using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using NameFixer.Core.Exceptions;

namespace NameFixer.Infrastructure.ExceptionHandling;

public class ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (Exception exp)
        {
            throw HandleExceptionAsync(exp);
        }
    }

    private RpcException HandleExceptionAsync(Exception exception)
    {
        logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        if (exception is ICustomMappedException e) return e.ToRpcException();

        var unknownStatus = new Status(StatusCode.Unknown, "Unknown error has occured.", exception);
        return new RpcException(unknownStatus);
    }
}