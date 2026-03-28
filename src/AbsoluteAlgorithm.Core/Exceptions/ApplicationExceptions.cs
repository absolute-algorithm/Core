using AbsoluteAlgorithm.Core.Constraints;

namespace AbsoluteAlgorithm.Core.Exceptions;

/// <summary>
/// Provides factory members for creating common <see cref="ApplicationException"/> instances.
/// </summary>
public static class ApplicationExceptions
{
    /// <summary>
    /// Gets an exception that represents an unauthorized request.
    /// </summary>
    public static ApplicationException Unauthorized => new ApplicationException(ERRORCODE.UNAUTHORIZED, "Unauthorized");

    /// <summary>
    /// Gets an exception that represents a forbidden request.
    /// </summary>
    public static ApplicationException Forbidden => new ApplicationException(ERRORCODE.FORBIDDEN, "Forbidden");

    /// <summary>
    /// Creates an exception that represents a missing resource.
    /// </summary>
    /// <param name="entity">The resource name to include in the error message.</param>
    /// <returns>An <see cref="ApplicationException"/> instance.</returns>
    public static ApplicationException Notfound(string entity) => new ApplicationException(ERRORCODE.NOTFOUND, string.Format("{0} not found.", entity));

    /// <summary>
    /// Creates an exception that represents an invalid request.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>An <see cref="ApplicationException"/> instance.</returns>
    public static ApplicationException Badrequest(string message) => new ApplicationException(ERRORCODE.BADREQUEST, message);

    /// <summary>
    /// Creates an exception that represents a failed request precondition.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>An <see cref="ApplicationException"/> instance.</returns>
    public static ApplicationException PreconditionFailed(string message) => new ApplicationException(ERRORCODE.PRECONDITIONFAILED, message);

    /// <summary>
    /// Creates an exception that represents an optimistic concurrency conflict.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>An <see cref="ApplicationException"/> instance.</returns>
    public static ApplicationException Conflict(string message) => new ApplicationException(ERRORCODE.CONFLICT, message);

    /// <summary>
    /// Creates an exception for the specified error code and message.
    /// </summary>
    /// <param name="code">The application-specific error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>An <see cref="ApplicationException"/> instance.</returns>
    public static ApplicationException FromCode(string code, string message) => new ApplicationException(code, message);
}