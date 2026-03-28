using System.Runtime.CompilerServices;
using System.Text.Json;
using AbsoluteAlgorithm.Core.Models.Configuration;
using AbsoluteAlgorithm.Core.Security;
using NLog;

namespace AbsoluteAlgorithm.Core.Diagnostics;

/// <summary>
/// A privacy-aware, high-performance logging wrapper for NLog.
/// Automatically handles PII redaction, route filtering, and logger blacklisting based on ApplicationConfiguration.
/// </summary>
public static class Logger
{
    private static LoggingConfiguration _config = new();

    /// <summary>
    /// Initializes the global logging state from the application configuration.
    /// This should be called once during project startup (Backend or Frontend).
    /// </summary>
    /// <param name="config">The logging configuration to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown if config is null.</exception>
    public static void Initialize(LoggingConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// Extracts the class name from the caller's file path.
    /// </summary>
    private static string GetName(string path) => System.IO.Path.GetFileNameWithoutExtension(path);

    #region Basic Logging

    /// <summary>
    /// Logs an informational message with optional area tagging.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="area">The logical area or component (optional).</param>
    /// <param name="path">The caller file path (auto-supplied by compiler).</param>
    public static void Info(string message, string? area = null, [CallerFilePath] string path = "")
    {
        if (!ShouldLog(path)) return;
        Log(LogLevel.Info, ProcessMessage(message), area, null, null, path);
    }

    /// <summary>
    /// Logs a warning message with optional area tagging.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    /// <param name="area">The logical area or component (optional).</param>
    /// <param name="path">The caller file path (auto-supplied by compiler).</param>
    public static void Warn(string message, string? area = null, [CallerFilePath] string path = "")
    {
        if (!ShouldLog(path)) return;
        Log(LogLevel.Warn, ProcessMessage(message), area, null, null, path);
    }

    /// <summary>
    /// Logs an error message with optional exception and error code.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="ex">The exception to log (optional).</param>
    /// <param name="errorCode">The error code to tag (optional).</param>
    /// <param name="path">The caller file path (auto-supplied by compiler).</param>
    public static void Error(string message, Exception? ex = null, string? errorCode = null, [CallerFilePath] string path = "")
    {
        if (!ShouldLog(path)) return;
        Log(LogLevel.Error, ProcessMessage(message), null, ex, errorCode, path);
    }

    #endregion

    #region Object Logging (PII Aware)

    /// <summary>
    /// Logs an object at the Info level by serializing it to JSON and redacting sensitive properties.
    /// </summary>
    public static void InfoObject<T>(string message, T data, [CallerFilePath] string path = "") where T : class
        => LogObject(LogLevel.Info, message, data, null, null, path);

    /// <summary>
    /// Logs an object at the Warn level. Useful for tracking suspicious activity or non-fatal failures.
    /// </summary>
    public static void WarnObject<T>(string message, T data, [CallerFilePath] string path = "") where T : class
        => LogObject(LogLevel.Warn, message, data, null, null, path);

    /// <summary>
    /// Logs an object at the Error level. Ideal for capturing the state of an object during a crash.
    /// </summary>
    public static void ErrorObject<T>(string message, T data, Exception? ex = null, string? errorCode = null, [CallerFilePath] string path = "") where T : class
        => LogObject(LogLevel.Error, message, data, ex, errorCode, path);

    /// <summary>
    /// Shared engine for object logging to ensure consistent serialization and redaction.
    /// </summary>
    public static void LogObject<T>(LogLevel level, string message, T data, Exception? ex, string? errorCode, string path) where T : class
    {
        if (!ShouldLog(path)) return;

        // Step 1: Serialize to JSON (Snapshot)
        string json = JsonSerializer.Serialize(data);
        
        // Step 2: Mask sensitive properties in the JSON string
        if (_config.EnablePiiRedaction && _config.RedactedProperties?.Any() == true)
        {
            json = Privacy.MaskProperties(json, _config.RedactedProperties);
        }

        // Step 3: Route to the main Log engine
        Log(level, $"{message} | Data: {json}", null, ex, errorCode, path);
    }
    #endregion

    #region Filtering & Validation Logic

    /// <summary>
    /// Determines if a log should be processed based on global enablement and class blacklisting.
    /// </summary>
    /// <param name="path">The caller file path.</param>
    /// <returns>True if logging should proceed; otherwise false.</returns>
    private static bool ShouldLog(string path)
    {
        if (!_config.EnableLogging) return false;

        string className = GetName(path);

        // Filter out noisy classes/loggers (e.g. "SignalRHeartbeat", "DbHeartbeat")
        if (_config.IgnoredLoggers?.Contains(className) == true) return false;

        return true;
    }

    /// <summary>
    /// Checks if a specific URL or route should be ignored for logging purposes.
    /// Used by Middleware (Backend) or HttpClients (Frontend) to skip /health or /auth endpoints.
    /// </summary>
    /// <param name="url">The URL or route to check.</param>
    /// <returns>True if the route is on the ignore list; otherwise false.</returns>
    public static bool IsRouteIgnored(string url)
    {
        if (string.IsNullOrEmpty(url) || _config.IgnoredRoutes == null) return false;
        
        return _config.IgnoredRoutes.Any(route => url.Contains(route, StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region Internal Engine

    /// <summary>
    /// Internal execution method that maps Absolute parameters to NLog's LogEventInfo.
    /// </summary>
    public static void Log(LogLevel level, string message, string? area, Exception? ex, string? errorCode, string path)
    {
        var name = GetName(path);
        var logEvent = new LogEventInfo(level, name, message)
        {
            Exception = ex
        };

        // Populate properties for structured JSON layout (Crucial for GCP/Cloud Logging)
        logEvent.Properties["Area"] = area ?? name;
        if (errorCode != null) logEvent.Properties["ErrorCode"] = errorCode;

        LogManager.GetLogger(name).Log(logEvent);
    }

    /// <summary>
    /// Scrubs the message for sensitive information (Emails/Phones) if PII redaction is enabled via Regex.
    /// </summary>
    private static string ProcessMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return message;

        return _config.EnablePiiRedaction
            ? Privacy.MaskSensitiveInformation(message)
            : message;
    }

    #endregion
}