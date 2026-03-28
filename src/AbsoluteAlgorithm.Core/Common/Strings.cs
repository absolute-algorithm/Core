using System;

namespace AbsoluteAlgorithm.Core.Common;

/// <summary>
/// Provides helper methods for string manipulation.
/// </summary>
public static class Strings
{
    /// <summary>
    /// Converts a string to snake_case.
    /// </summary>
    public static string ToSnakeCase(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input.Select((ch, i) =>
            char.IsUpper(ch) && i > 0 ? "_" + char.ToLower(ch) : char.ToLower(ch).ToString()));
    }

    /// <summary>
    /// Converts a string to kebab-case.
    /// </summary>
    public static string ToKebabCase(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input.Select((ch, i) =>
            char.IsUpper(ch) && i > 0 ? "-" + char.ToLower(ch) : char.ToLower(ch).ToString()));
    }

    /// <summary>
    /// Checks if a string is a palindrome.
    /// </summary>
    public static bool IsPalindrome(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var normalized = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return normalized.SequenceEqual(normalized.Reverse());
    }

    /// <summary>
    /// Converts a string to PascalCase.
    /// </summary>
    public static string ToPascalCase(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }
}
