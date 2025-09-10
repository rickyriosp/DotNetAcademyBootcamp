using System.Diagnostics;

using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.Api.Shared.ErrorHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    // In order to turn a class into a global exception handler we need to implement interface: IExceptionHandler

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.TraceId;

        logger.LogError(exception,
            "Could not process on machine {Machine}. TraceId: {TraceId}",
            Environment.MachineName, traceId);

        await Results.Problem(
            title: "Error occurred while processing your request",
            statusCode: StatusCodes.Status500InternalServerError,
            extensions: new Dictionary<string, object?> { { "traceId", traceId.ToString() } }
        ).ExecuteAsync(httpContext);

        return true;
    }
}