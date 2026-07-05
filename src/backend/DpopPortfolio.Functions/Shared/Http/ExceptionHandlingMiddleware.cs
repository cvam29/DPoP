using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace DpopPortfolio.Functions.Shared.Http;

public sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled function exception for invocation {InvocationId}.", context.InvocationId);

            var request = await context.GetHttpRequestDataAsync();
            if (request is null)
            {
                throw;
            }

            var response = await JsonResponse.CreateErrorAsync(
                request,
                HttpStatusCode.InternalServerError,
                "internal_server_error",
                "An unexpected server error occurred.",
                context);

            context.GetInvocationResult().Value = response;
        }
    }
}
