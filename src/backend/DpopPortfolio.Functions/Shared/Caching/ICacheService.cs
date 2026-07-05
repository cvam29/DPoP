namespace DpopPortfolio.Functions.Shared.Caching;

public interface ICacheService
{
    ValueTask<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
        where T : notnull;
}
