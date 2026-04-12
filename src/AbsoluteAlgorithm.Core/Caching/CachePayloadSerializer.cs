using System.Text;
using AbsoluteAlgorithm.Core.Serialization;

namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// Provides serialization helpers for caching payloads.
/// </summary>
public static class CachePayloadSerializer
{
    /// <summary>
    /// Serializes a value into UTF-8 bytes.
    /// </summary>
    public static byte[] Serialize<T>(T value)
    {
        var json = Json.Serialize(value);
        return Encoding.UTF8.GetBytes(json);
    }

    /// <summary>
    /// Deserializes a value from UTF-8 bytes.
    /// </summary>
    public static T? Deserialize<T>(byte[] value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var json = Encoding.UTF8.GetString(value);
        return Json.Deserialize<T>(json);
    }
}
