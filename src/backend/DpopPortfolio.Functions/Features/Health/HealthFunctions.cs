using System.Net;
using DpopPortfolio.Functions.Shared.Configuration;
using DpopPortfolio.Functions.Shared.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DpopPortfolio.Functions.Features.Health;

public sealed class HealthFunctions(
    IOptions<DpopPortfolioOptions> options,
    IHostEnvironment hostEnvironment)
{
    [Function(nameof(GetHealth))]
    public Task<HttpResponseData> GetHealth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData request)
    {
        var settings = options.Value;

        return JsonResponse.CreateAsync(
            request,
            HttpStatusCode.OK,
            new
            {
                status = "healthy",
                service = "dpop-portfolio-functions",
                environment = hostEnvironment.EnvironmentName,
                utc = DateTimeOffset.UtcNow,
                configuration = new
                {
                    issuer = settings.Issuer,
                    audience = settings.Audience,
                    frontendOrigin = settings.FrontendOrigin,
                    cacheProvider = settings.CacheProvider,
                    storageProvider = settings.StorageProvider,
                    tokenLifetimes = settings.TokenLifetimes
                }
            });
    }
}
