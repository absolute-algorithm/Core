using System;

namespace AbsoluteAlgorithm.Core.Extensions;

/// <summary>
/// Provides extension methods for string manipulation.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates a string to a specified maximum length and appends "..." if it was truncated.
    /// </summary>
    public static string Truncate(this string input, int maxLength)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        if (input.Length <= maxLength) return input;

        return input.Substring(0, maxLength - 3) + "...";
    }

    /// <summary>
    /// Converts a string to snake_case.
    /// </summary>
    public static string ToSnakeCase(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input.Select((ch, i) =>
            char.IsUpper(ch) && i > 0 ? "_" + char.ToLower(ch) : char.ToLower(ch).ToString()));
    }

    /// <summary>
    /// Converts a string to kebab-case.
    /// </summary>
    public static string ToKebabCase(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input.Select((ch, i) =>
            char.IsUpper(ch) && i > 0 ? "-" + char.ToLower(ch) : char.ToLower(ch).ToString()));
    }

    /// <summary>
    /// Converts a string to PascalCase.
    /// </summary>
    public static string ToPascalCase(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }

    /// <summary>
    /// Checks if a string is a palindrome.
    /// </summary>
    public static bool IsPalindrome(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var normalized = new string(input.Where(char.IsLetterOrDigit).ToArray()).ToLower();
        return normalized.SequenceEqual(normalized.Reverse());
    }

}
