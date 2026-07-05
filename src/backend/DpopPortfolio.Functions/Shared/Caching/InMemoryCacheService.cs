using System.Collections.Concurrent;

namespace DpopPortfolio.Functions.Shared.Caching;

public sealed class InMemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _entries = new();

    public async ValueTask<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        var now = DateTimeOffset.UtcNow;

        if (_entries.TryGetValue(key, out var cached)
            && cached.ExpiresAt > now
            && cached.Value is T cachedValue)
        {
            return cachedValue;
        }

        var value = await factory(cancellationToken);
        _entries[key] = new CacheEntry(value, now.Add(ttl));

        return value;
    }

    private sealed record CacheEntry(object Value, DateTimeOffset ExpiresAt);
}
