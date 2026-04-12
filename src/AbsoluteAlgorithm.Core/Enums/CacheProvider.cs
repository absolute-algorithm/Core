namespace AbsoluteAlgorithm.Core.Enums;

/// <summary>
/// Specifies the cache provider.
/// </summary>
public enum CacheProvider : byte
{
    /// <summary>
    /// In-process memory cache.
    /// </summary>
    Memory = 1,

    /// <summary>
    /// Distributed cache backend.
    /// </summary>
    Distributed
}
