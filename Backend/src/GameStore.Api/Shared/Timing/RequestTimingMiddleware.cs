using System.Diagnostics;

namespace GameStore.Api.Shared.Timing;

// Primary constructor
public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    // Middleware class needs a public constructor with a RequestDelegate parameter
    // Needs to have an Invoke or InvokeAsync public method

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();

            await next(context);
        }
        finally
        {
            stopWatch.Stop();

            var elapsedMs = stopWatch.ElapsedMilliseconds;
            logger.LogInformation("{RequestMethod} {RequestPath} completed with status {Status} in {elapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsedMs);
        }
    }
}