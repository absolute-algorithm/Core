namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// Default idempotency cache store implementation that delegates to <see cref="IApplicationCache"/>.
/// </summary>
public class IdempotencyCacheStore : IIdempotencyCacheStore
{
    private readonly IApplicationCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdempotencyCacheStore"/> class.
    /// </summary>
    public IdempotencyCacheStore(IApplicationCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc />
    public Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.GetAsync<string>(key, cancellationToken);
    }

    /// <inheritdoc />
    public Task SetAsync(string key, string payload, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        return _cache.SetAsync(key, payload, expiration, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }
}
