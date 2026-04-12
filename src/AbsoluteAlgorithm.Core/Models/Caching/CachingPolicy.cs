using System.Text.Json.Serialization;
using AbsoluteAlgorithm.Core.Enums;

namespace AbsoluteAlgorithm.Core.Models.Caching;

/// <summary>
/// Represents global caching configuration used by library features.
/// </summary>
public class CachingPolicy
{
    /// <summary>
    /// Gets a value indicating whether caching is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;

    /// <summary>
    /// Gets the cache provider.
    /// </summary>
    [JsonPropertyName("provider")]
    public CacheProvider Provider { get; init; } = CacheProvider.Memory;

    /// <summary>
    /// Gets the cache topology.
    /// </summary>
    [JsonPropertyName("scope")]
    public CacheScope Scope { get; init; } = CacheScope.LocalMemory;

    /// <summary>
    /// Gets the default absolute expiration value, in seconds.
    /// </summary>
    [JsonPropertyName("defaultAbsoluteExpirationSeconds")]
    public int DefaultAbsoluteExpirationSeconds { get; init; } = 300;

    /// <summary>
    /// Gets the default sliding expiration value, in seconds.
    /// </summary>
    [JsonPropertyName("defaultSlidingExpirationSeconds")]
    public int? DefaultSlidingExpirationSeconds { get; init; }

    /// <summary>
    /// Gets the optional environment variable name that points to an external cache connection string.
    /// </summary>
    [JsonPropertyName("connectionStringName")]
    public string? ConnectionStringName { get; init; }

    /// <summary>
    /// Gets the optional cache instance name used by distributed providers.
    /// </summary>
    [JsonPropertyName("instanceName")]
    public string? InstanceName { get; init; }

    /// <summary>
    /// Gets the optional named cache entry profiles.
    /// </summary>
    [JsonPropertyName("entryPolicies")]
    public IReadOnlyList<CacheEntryPolicy> EntryPolicies { get; init; } = Array.Empty<CacheEntryPolicy>();
}
