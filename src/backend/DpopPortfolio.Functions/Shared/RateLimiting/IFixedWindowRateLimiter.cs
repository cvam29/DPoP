namespace DpopPortfolio.Functions.Shared.RateLimiting;

public interface IFixedWindowRateLimiter
{
    ValueTask<RateLimitResult> CheckAsync(
        string key,
        int permitLimit,
        TimeSpan window,
        CancellationToken cancellationToken = default);
}
