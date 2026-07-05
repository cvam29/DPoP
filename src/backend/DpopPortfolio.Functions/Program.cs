using DpopPortfolio.Functions.Shared.Caching;
using DpopPortfolio.Functions.Shared.Configuration;
using DpopPortfolio.Functions.Shared.Http;
using DpopPortfolio.Functions.Shared.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, configuration) =>
    {
        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<ExceptionHandlingMiddleware>();
        worker.UseMiddleware<RequestLoggingMiddleware>();
        worker.UseMiddleware<CorsMiddleware>();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IValidateOptions<DpopPortfolioOptions>, DpopPortfolioOptionsValidator>();
        services
            .AddOptions<DpopPortfolioOptions>()
            .Bind(context.Configuration.GetSection(DpopPortfolioOptions.SectionName))
            .PostConfigure(options => options.ApplyEnvironmentFallbacks())
            .ValidateOnStart();

        services.AddSingleton<ICacheService, InMemoryCacheService>();
        services.AddSingleton<IFixedWindowRateLimiter, InMemoryFixedWindowRateLimiter>();
    })
    .Build();

host.Run();
