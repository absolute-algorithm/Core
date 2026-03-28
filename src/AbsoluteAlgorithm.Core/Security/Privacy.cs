using System.Text.Json;
using System.Text.RegularExpressions;
using System.Reflection;

namespace AbsoluteAlgorithm.Core.Security;

/// <summary>
/// Provides utility methods for handling Personally Identifiable Information (PII).
/// </summary>
public static class Privacy
{
    /// <summary>
    /// Masks sensitive information in a string, such as email addresses and phone numbers.
    /// </summary>
    /// <param name="input">The input string containing PII.</param>
    /// <returns>The input string with sensitive information masked.</returns>
    public static string MaskSensitiveInformation(string input)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(input);

        // Mask email addresses
        string maskedEmails = Regex.Replace(input, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", "***@***.***");

        // Mask phone numbers
        string maskedPhones = Regex.Replace(maskedEmails, @"\b\d{3}[-.\s]?\d{2}[-.\s]?\d{4}\b", "***-***-****");

        return maskedPhones;
    }

    /// <summary>
    /// Validates if a given string contains PII such as email addresses or phone numbers.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <returns><see langword="true"/> if PII is detected; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsPII(string input)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(input);

        // Check for email addresses
        bool hasEmail = Regex.IsMatch(input, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

        // Check for phone numbers
        bool hasPhone = Regex.IsMatch(input, @"\b\d{3}[-.\s]?\d{2}[-.\s]?\d{4}\b");

        return hasEmail || hasPhone;
    }

    /// <summary>
    /// Masks sensitive information in a JSON string based on specified property names.
    /// </summary>
    /// <param name="json">The JSON string containing PII.</param>
    /// <param name="propertyNames">The list of property names to mask.</param>
    /// <returns>The JSON string with specified properties masked.</returns>
    public static string MaskProperties(string json, IEnumerable<string> propertyNames)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(json);
        ArgumentNullException.ThrowIfNull(propertyNames);

        try
        {
            var jsonDocument = JsonDocument.Parse(json);
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            if (jsonObject is not null)
            {
                foreach (var propertyName in propertyNames)
                {
                    if (jsonObject.ContainsKey(propertyName))
                    {
                        jsonObject[propertyName] = "***MASKED***";
                    }
                }
            }

            return JsonSerializer.Serialize(jsonObject);
        }
        catch (JsonException)
        {
            // Return the original JSON if parsing fails
            return json;
        }
    }

    /// <summary>
    /// Masks sensitive information in an object based on specified property names using reflection.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object containing PII.</param>
    /// <param name="propertyNames">The list of property names to mask.</param>
    /// <returns>The object with specified properties masked.</returns>
    public static T MaskProperties<T>(T obj, IEnumerable<string> propertyNames) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(propertyNames);

        foreach (var propertyName in propertyNames)
        {
            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite && property.PropertyType == typeof(string))
            {
                var currentValue = property.GetValue(obj) as string;
                if (!string.IsNullOrEmpty(currentValue))
                {
                    property.SetValue(obj, "***MASKED***");
                }
            }
        }

        return obj;
    }
}
