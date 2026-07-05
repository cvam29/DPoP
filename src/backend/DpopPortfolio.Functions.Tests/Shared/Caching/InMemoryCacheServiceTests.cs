using DpopPortfolio.Functions.Shared.Caching;

namespace DpopPortfolio.Functions.Tests.Shared.Caching;

public sealed class InMemoryCacheServiceTests
{
    [Fact]
    public async Task GetOrCreateAsync_ReturnsCachedValue_WithinTtl()
    {
        var cache = new InMemoryCacheService();
        var factoryCalls = 0;

        var first = await cache.GetOrCreateAsync(
            "catalog:v1",
            _ =>
            {
                factoryCalls++;
                return ValueTask.FromResult("first-value");
            },
            TimeSpan.FromMinutes(5));

        var second = await cache.GetOrCreateAsync(
            "catalog:v1",
            _ =>
            {
                factoryCalls++;
                return ValueTask.FromResult("second-value");
            },
            TimeSpan.FromMinutes(5));

        Assert.Equal("first-value", first);
        Assert.Equal("first-value", second);
        Assert.Equal(1, factoryCalls);
    }

    [Fact]
    public async Task GetOrCreateAsync_RebuildsValue_AfterTtlExpires()
    {
        var cache = new InMemoryCacheService();

        var first = await cache.GetOrCreateAsync(
            "catalog:v2",
            _ => ValueTask.FromResult("first-value"),
            TimeSpan.FromMilliseconds(-1));

        var second = await cache.GetOrCreateAsync(
            "catalog:v2",
            _ => ValueTask.FromResult("second-value"),
            TimeSpan.FromMinutes(5));

        Assert.Equal("first-value", first);
        Assert.Equal("second-value", second);
    }
}
