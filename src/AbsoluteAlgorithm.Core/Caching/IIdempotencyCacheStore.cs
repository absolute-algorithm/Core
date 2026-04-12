namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// Defines storage operations for idempotency replay payloads.
/// </summary>
public interface IIdempotencyCacheStore
{
    /// <summary>
    /// Gets the replay payload for an idempotency key.
    /// </summary>
    Task<string?> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores the replay payload for an idempotency key.
    /// </summary>
    Task SetAsync(string key, string payload, TimeSpan expiration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a replay payload for an idempotency key.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
