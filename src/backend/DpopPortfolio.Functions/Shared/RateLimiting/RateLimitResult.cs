namespace DpopPortfolio.Functions.Shared.RateLimiting;

public sealed record RateLimitResult(
    bool IsAllowed,
    int Remaining,
    DateTimeOffset ResetAt,
    TimeSpan RetryAfter);
