using System.Text.Json.Serialization;

namespace AbsoluteAlgorithm.Core.Models.Caching;

/// <summary>
/// Represents a named cache entry profile that can override default expiration settings.
/// </summary>
public class CacheEntryPolicy
{
    /// <summary>
    /// Gets the unique cache profile name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets a value indicating whether this cache profile is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;

    /// <summary>
    /// Gets the absolute expiration value, in seconds.
    /// </summary>
    [JsonPropertyName("absoluteExpirationSeconds")]
    public int AbsoluteExpirationSeconds { get; init; } = 300;

    /// <summary>
    /// Gets the sliding expiration value, in seconds.
    /// </summary>
    [JsonPropertyName("slidingExpirationSeconds")]
    public int? SlidingExpirationSeconds { get; init; }
}
