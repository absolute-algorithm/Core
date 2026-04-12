namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// Defines a simple typed cache abstraction for application features.
/// </summary>
public interface IApplicationCache
{
    /// <summary>
    /// Gets a cached value by key.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a cached value using expiration settings.
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached value.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
