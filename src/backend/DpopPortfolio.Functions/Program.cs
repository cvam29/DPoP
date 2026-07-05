using DpopPortfolio.Functions.Shared.Caching;
using DpopPortfolio.Functions.Shared.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<ICacheService, InMemoryCacheService>();
        services.AddSingleton<IFixedWindowRateLimiter, InMemoryFixedWindowRateLimiter>();
    })
    .Build();

host.Run();
