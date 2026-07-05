using System.Diagnostics;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace DpopPortfolio.Functions.Shared.Http;

public sealed class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var request = await context.GetHttpRequestDataAsync();
        var startedAt = Stopwatch.GetTimestamp();

        await next(context);

        if (request is null)
        {
            return;
        }

        var elapsed = Stopwatch.GetElapsedTime(startedAt);
        logger.LogInformation(
            "HTTP {Method} {Path} completed in {ElapsedMilliseconds} ms for invocation {InvocationId}.",
            request.Method,
            request.Url.AbsolutePath,
            elapsed.TotalMilliseconds,
            context.InvocationId);
    }
}
