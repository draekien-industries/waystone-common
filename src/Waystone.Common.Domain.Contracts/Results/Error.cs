namespace Waystone.Common.Domain.Contracts.Results;

using System.Net;

/// <summary>
/// An error that occured during the normal operation of the application.
/// Used inside the <see cref="Result" /> object.
/// </summary>
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

    public string Code { get; }

    public string Message { get; }

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

    public HttpStatusCode HttpStatusCode { get; }
}
