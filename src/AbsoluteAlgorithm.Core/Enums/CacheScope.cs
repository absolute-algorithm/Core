namespace AbsoluteAlgorithm.Core.Enums;

/// <summary>
/// Specifies the cache topology used by the application.
/// </summary>
public enum CacheScope : byte
{
    /// <summary>
    /// In-process cache only.
    /// </summary>
    LocalMemory = 1,

    /// <summary>
    /// External distributed cache only.
    /// </summary>
    Distributed,

    /// <summary>
    /// Combination of local memory and distributed cache.
    /// </summary>
    Hybrid
}
