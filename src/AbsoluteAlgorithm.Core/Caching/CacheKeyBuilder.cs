using System.Security.Cryptography;
using System.Text;

namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// Builds normalized cache keys for common library scenarios.
/// </summary>
public static class CacheKeyBuilder
{
    /// <summary>
    /// Builds a deterministic cache key by joining parts with ':'.
    /// </summary>
    public static string Build(params string[] parts)
    {
        ArgumentNullException.ThrowIfNull(parts);

        var normalized = parts
            .Where(static part => !string.IsNullOrWhiteSpace(part))
            .Select(static part => part.Trim().ToLowerInvariant());

        return string.Join(':', normalized);
    }

    /// <summary>
    /// Builds an idempotency cache key from request metadata.
    /// </summary>
    public static string BuildIdempotencyKey(string method, string path, string idempotencyKey, string? queryString = null)
    {
        var fingerprint = Build(method, path, queryString ?? string.Empty, idempotencyKey);
        return $"idempotency:{Hash(fingerprint)}";
    }

    private static string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
