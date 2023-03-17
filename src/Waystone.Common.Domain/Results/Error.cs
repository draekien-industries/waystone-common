namespace Waystone.Common.Domain.Results;

using System.Net;

/// <summary>
/// An error that occured during the normal operation of the application.
/// Used inside the <see cref="Result" /> object.
/// </summary>
[PublicAPI]
public class Error
{
    /// <summary>
    /// An error that occured during the normal operation of the application.
    /// Used inside the <see cref="Result" /> object.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exception">An optional exception which caused this error.</param>
    public Error(string code, string message, Exception? exception = default)
    {
        Code = code;
        Message = message;
        Exception = exception;
    }

    /// <summary>
    /// The code associated with the error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// The message associated with the error.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// The exception which caused the error.
    /// </summary>
    public Exception? Exception { get; }
}

/// <summary>
/// An error that can be mapped to a <see cref="HttpStatusCode" />.
/// </summary>
public class HttpError : Error
{
    /// <summary>
    /// An error that can be mapped to a <see cref="HttpStatusCode" />.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP Status Code to map the error to.</param>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exception">An optional exception which caused this error.</param>
    public HttpError(HttpStatusCode httpStatusCode, string code, string message, Exception? exception = default) : base(
        code,
        message,
        exception)
    {
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// An error that can be mapped to a <see cref="HttpStatusCode" />.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP Status Code to map the error to.</param>
    /// <param name="error">The base error.</param>
    public HttpError(HttpStatusCode httpStatusCode, Error error) : base(error.Code, error.Message, error.Exception)
    {
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// The HTTP Status Code this error should be mapped to.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }
}
