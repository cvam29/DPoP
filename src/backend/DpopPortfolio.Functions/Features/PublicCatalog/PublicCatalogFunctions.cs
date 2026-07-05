using System.Net;
using DpopPortfolio.Functions.Shared.Caching;
using DpopPortfolio.Functions.Shared.Http;
using DpopPortfolio.Functions.Shared.RateLimiting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace DpopPortfolio.Functions.Features.PublicCatalog;

public sealed class PublicCatalogFunctions(
    ICacheService cache,
    IFixedWindowRateLimiter rateLimiter)
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan RateLimitWindow = TimeSpan.FromMinutes(1);

    [Function(nameof(GetPublicCatalog))]
    public async Task<HttpResponseData> GetPublicCatalog(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/public/catalog")] HttpRequestData request)
    {
        var clientKey = GetClientKey(request);
        var rateLimit = await rateLimiter.CheckAsync($"public-catalog:{clientKey}", permitLimit: 20, window: RateLimitWindow);

        if (!rateLimit.IsAllowed)
        {
            var blocked = await JsonResponse.CreateAsync(
                request,
                HttpStatusCode.TooManyRequests,
                new
                {
                    error = "rate_limit_exceeded",
                    message = "The public catalog allows 20 requests per minute per client in the starter implementation."
                });

            ApplyRateLimitHeaders(blocked, rateLimit, limit: 20);
            return blocked;
        }

        var catalog = await cache.GetOrCreateAsync(
            "public-catalog:v1",
            _ => ValueTask.FromResult(GetSeedCatalog()),
            CacheDuration);

        var response = await JsonResponse.CreateAsync(
            request,
            HttpStatusCode.OK,
            new
            {
                cache = new
                {
                    provider = "in-memory",
                    ttlSeconds = (int)CacheDuration.TotalSeconds,
                    portfolioNote = "Redis-backed distributed caching is planned for the next infrastructure milestone."
                },
                catalog
            });

        response.Headers.Add("Cache-Control", "public, max-age=60");
        ApplyRateLimitHeaders(response, rateLimit, limit: 20);
        return response;
    }

    private static IReadOnlyList<CatalogItem> GetSeedCatalog()
    {
        return
        [
            new("dpop-flow", "DPoP OAuth Flow", "Authorization Code + PKCE with sender-constrained tokens."),
            new("replay-cache", "Replay Cache", "Proof jti values are cached to prevent replay attacks."),
            new("rate-limit", "Rate Limiting", "Token and API endpoints use fixed-window limits in the starter slice."),
            new("authorization", "Authorization", "Scopes and roles protect resource APIs.")
        ];
    }

    private static string GetClientKey(HttpRequestData request)
    {
        if (request.Headers.TryGetValues("x-forwarded-for", out var forwardedFor))
        {
            var firstForwardedAddress = forwardedFor.FirstOrDefault()?.Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(firstForwardedAddress))
            {
                return firstForwardedAddress;
            }
        }

        return request.Url.Host;
    }

    private static void ApplyRateLimitHeaders(HttpResponseData response, RateLimitResult result, int limit)
    {
        response.Headers.Add("X-RateLimit-Limit", limit.ToString());
        response.Headers.Add("X-RateLimit-Remaining", result.Remaining.ToString());
        response.Headers.Add("X-RateLimit-Reset", result.ResetAt.ToUnixTimeSeconds().ToString());

        if (!result.IsAllowed)
        {
            response.Headers.Add("Retry-After", Math.Ceiling(result.RetryAfter.TotalSeconds).ToString());
        }
    }

    private sealed record CatalogItem(string Id, string Name, string Description);
}
