using System.Security.Cryptography;
using System.Numerics;

namespace AbsoluteAlgorithm.Core.Numerics;

/// <summary>
/// Provides thread-safe, high-performance, and cryptographically secure randomness.
/// Designed for both Game Logic (Stride/Unity) and Backend Security.
/// </summary>
public static class Randomness
{
    // ThreadStatic ensures each thread has its own instance to avoid lock contention.
    [ThreadStatic] 
    private static Random? _localRandom;
    private static Random Instance => _localRandom ??= new Random();

    #region Basic Random Generation

    /// <summary>
    /// Returns a random integer between min (inclusive) and max (exclusive).
    /// </summary>
    public static int Next(int min, int max) => Instance.Next(min, max);

    /// <summary>
    /// Returns a random generic floating-point number between 0.0 and 1.0.
    /// </summary>
    public static T NextFloat<T>() where T : IFloatingPoint<T> 
        => T.CreateChecked(Instance.NextDouble());

    /// <summary>
    /// Returns a random element from a collection.
    /// </summary>
    public static T Pick<T>(IList<T> items)
    {
        if (items.Count == 0) throw new ArgumentException("Collection is empty.");
        return items[Instance.Next(items.Count)];
    }

    /// <summary>
    /// Returns true based on a probability between 0.0 and 1.0.
    /// Example: Chance(0.15f) returns true 15% of the time. Useful for "Wicket" checks.
    /// </summary>
    public static bool Chance<T>(T probability) where T : IFloatingPoint<T>
    {
        return NextFloat<T>() < probability;
    }

    /// <summary>
    /// Returns a random integer from a specific set of options.
    /// Example: Pick(0, 1, 2, 3, 4, 6) returns one of the provided options.
    /// </summary>
    public static int Pick(params int[] options)
    {
        return options[Instance.Next(options.Length)];
    }

    #endregion

    #region Weighted Randomness (The "Gacha" Logic)

    /// <summary>
    /// Picks an item based on its weight. Higher weights have a higher chance of being picked.
    /// Useful for Game Loot, XP drops, or weighted Load Balancing.
    /// </summary>
    public static T PickWeighted<T>(IList<T> items, IList<double> weights)
    {
        if (items.Count != weights.Count) throw new ArgumentException("Items and weights must match.");

        double totalWeight = 0;
        foreach (var w in weights) totalWeight += w;

        double roll = Instance.NextDouble() * totalWeight;
        double currentSum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            currentSum += weights[i];
            if (roll <= currentSum) return items[i];
        }

        return items[^1];
    }

    #endregion

    #region Security & Tokens

    /// <summary>
    /// Generates a cryptographically secure random string.
    /// Essential for API Keys, Password Reset Tokens, or Session IDs.
    /// </summary>
    public static string GenerateToken(int length = 32)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return string.Create(length, chars, (span, alphabet) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];
            }
        });
    }

    #endregion

    #region Spatial Randomness

    /// <summary>
    /// Returns a random 2D direction (Unit Vector).
    /// </summary>
    public static Vector2 InsideUnitCircle()
    {
        float angle = NextFloat<float>() * MathF.PI * 2f;
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
    }

    #endregion
}