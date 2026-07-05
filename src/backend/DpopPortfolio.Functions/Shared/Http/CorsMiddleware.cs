using System.Net;
using DpopPortfolio.Functions.Shared.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;

namespace DpopPortfolio.Functions.Shared.Http;

public sealed class CorsMiddleware(IOptions<DpopPortfolioOptions> options) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var request = await context.GetHttpRequestDataAsync();

        if (request is not null
            && request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = request.CreateResponse(HttpStatusCode.NoContent);
            CorsHeaders.Apply(preflightResponse, options.Value.FrontendOrigin);
            context.GetInvocationResult().Value = preflightResponse;
            return;
        }

        await next(context);

        var response = context.GetInvocationResult().Value as HttpResponseData;
        if (response is not null)
        {
            CorsHeaders.Apply(response, options.Value.FrontendOrigin);
        }
    }
}
