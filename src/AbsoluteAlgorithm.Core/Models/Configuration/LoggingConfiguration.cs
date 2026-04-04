using System;
using System.Text.Json.Serialization;

namespace AbsoluteAlgorithm.Core.Models.Configuration;

/// <summary>
/// Defines how the Absolute Logging system behaves across the application.
/// </summary>
public class LoggingConfiguration
{
    /// <summary>
    /// Master switch for logging services.
    /// </summary>
    [JsonPropertyName("enableLogging")]
    public bool EnableLogging { get; init; } = true;

    /// <summary>
    /// If true, the logger will capture and log the full HTTP request and response bodies for API calls. Use with caution in production environments due to potential performance impacts and sensitive data exposure. use PII redaction features to mitigate risks.
    /// </summary>
    [JsonPropertyName("enableRequestAndResponseLogging")]
    public bool EnableRequestAndResponseLogging { get; init; } = false;

    /// <summary>
    /// If true, the logger will scan strings and objects for PII and mask them before writing to NLog.
    /// </summary>
    [JsonPropertyName("enablePiiRedaction")]
    public bool EnablePiiRedaction { get; init; } = false;

    /// <summary>
    /// Specific JSON/Object property names that must ALWAYS be masked (e.g., "Password", "Ssn", "Email").
    /// </summary>
    [JsonPropertyName("redactedProperties")]
    public List<string> RedactedProperties { get; init; } = new();

    /// <summary>
    /// List of API endpoints or URLs to skip logging for (e.g., "/health", "/login").
    /// </summary>
    [JsonPropertyName("ignoredRoutes")]
    public List<string> IgnoredRoutes { get; init; } = new();

    /// <summary>
    /// List of Logger names (class names) to skip (e.g., "SignalRHeartbeat").
    /// </summary>
    [JsonPropertyName("ignoredLoggers")]
    public List<string> IgnoredLoggers { get; init; } = new();
}