using System.Text.Json.Serialization;

namespace AbsoluteAlgorithm.Core.Exceptions;

/// <summary>
/// Represents an application exception that is translated directly to the standard API error response.
/// </summary>
public class ApplicationException : System.ApplicationException
{
    /// <summary>
    /// Gets the application-specific error code.
    /// </summary>
    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; init; }

    /// <summary>
    /// Gets the error message returned to the client.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class.
    /// </summary>
    /// <param name="errorCode">The standardized application error code.</param>
    /// <param name="errorMessage">The message to return to the client.</param>
    public ApplicationException(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}
