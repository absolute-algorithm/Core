using Microsoft.Extensions.Caching.Memory;

namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// In-process <see cref="IApplicationCache"/> implementation backed by <see cref="IMemoryCache"/>.
/// </summary>
public class MemoryApplicationCache : IApplicationCache
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryApplicationCache"/> class.
    /// </summary>
    public MemoryApplicationCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration
        };

        if (slidingExpiration.HasValue)
        {
            options.SlidingExpiration = slidingExpiration;
        }

        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _cache.Remove(key);
        return Task.CompletedTask;
    }
}
