using DpopPortfolio.Functions.Shared.RateLimiting;

namespace DpopPortfolio.Functions.Tests.Shared.RateLimiting;

public sealed class InMemoryFixedWindowRateLimiterTests
{
    [Fact]
    public async Task CheckAsync_AllowsRequests_UntilPermitLimitIsExceeded()
    {
        var limiter = new InMemoryFixedWindowRateLimiter();

        var first = await limiter.CheckAsync("client-a", permitLimit: 2, window: TimeSpan.FromMinutes(1));
        var second = await limiter.CheckAsync("client-a", permitLimit: 2, window: TimeSpan.FromMinutes(1));
        var third = await limiter.CheckAsync("client-a", permitLimit: 2, window: TimeSpan.FromMinutes(1));

        Assert.True(first.IsAllowed);
        Assert.Equal(1, first.Remaining);
        Assert.True(second.IsAllowed);
        Assert.Equal(0, second.Remaining);
        Assert.False(third.IsAllowed);
        Assert.Equal(0, third.Remaining);
        Assert.True(third.RetryAfter > TimeSpan.Zero);
    }

    [Fact]
    public async Task CheckAsync_TracksClientsIndependently()
    {
        var limiter = new InMemoryFixedWindowRateLimiter();

        var firstClient = await limiter.CheckAsync("client-a", permitLimit: 1, window: TimeSpan.FromMinutes(1));
        var secondClient = await limiter.CheckAsync("client-b", permitLimit: 1, window: TimeSpan.FromMinutes(1));
        var blockedFirstClient = await limiter.CheckAsync("client-a", permitLimit: 1, window: TimeSpan.FromMinutes(1));

        Assert.True(firstClient.IsAllowed);
        Assert.True(secondClient.IsAllowed);
        Assert.False(blockedFirstClient.IsAllowed);
    }
}
