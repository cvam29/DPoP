using DpopPortfolio.Functions.Shared.Caching;
using DpopPortfolio.Functions.Shared.Configuration;
using DpopPortfolio.Functions.Shared.Http;
using DpopPortfolio.Functions.Shared.RateLimiting;
using DpopPortfolio.ServiceDefaults;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.UseFunctionsMiddleware<ExceptionHandlingMiddleware>();
builder.UseFunctionsMiddleware<RequestLoggingMiddleware>();
builder.UseFunctionsMiddleware<CorsMiddleware>();

builder.Services.AddSingleton<IValidateOptions<DpopPortfolioOptions>, DpopPortfolioOptionsValidator>();
builder.Services
    .AddOptions<DpopPortfolioOptions>()
    .Bind(builder.Configuration.GetSection(DpopPortfolioOptions.SectionName))
    .PostConfigure(options => options.ApplyEnvironmentFallbacks())
    .ValidateOnStart();

builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
builder.Services.AddSingleton<IFixedWindowRateLimiter, InMemoryFixedWindowRateLimiter>();

builder.Build().Run();

internal static class FunctionsApplicationBuilderExtensions
{
    public static FunctionsApplicationBuilder UseFunctionsMiddleware<TMiddleware>(
        this FunctionsApplicationBuilder builder)
        where TMiddleware : class, IFunctionsWorkerMiddleware
    {
        builder.Services.AddSingleton<TMiddleware>();
        builder.Use(next => async context =>
        {
            var middleware = context.InstanceServices.GetRequiredService<TMiddleware>();
            await middleware.Invoke(context, next);
        });

        return builder;
    }
}
