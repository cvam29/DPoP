using System.Collections.Concurrent;

namespace DpopPortfolio.Functions.Shared.RateLimiting;

public sealed class InMemoryFixedWindowRateLimiter : IFixedWindowRateLimiter
{
    private readonly ConcurrentDictionary<string, WindowCounter> _counters = new();

    public ValueTask<RateLimitResult> CheckAsync(
        string key,
        int permitLimit,
        TimeSpan window,
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var counter = _counters.AddOrUpdate(
            key,
            _ => new WindowCounter(Count: 1, ResetAt: now.Add(window)),
            (_, existing) => existing.ResetAt <= now
                ? new WindowCounter(Count: 1, ResetAt: now.Add(window))
                : existing with { Count = existing.Count + 1 });

        var remaining = Math.Max(permitLimit - counter.Count, 0);
        var isAllowed = counter.Count <= permitLimit;
        var retryAfter = isAllowed ? TimeSpan.Zero : counter.ResetAt - now;

        return ValueTask.FromResult(new RateLimitResult(isAllowed, remaining, counter.ResetAt, retryAfter));
    }

    private sealed record WindowCounter(int Count, DateTimeOffset ResetAt);
}
