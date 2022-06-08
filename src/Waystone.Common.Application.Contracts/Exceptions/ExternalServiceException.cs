namespace Waystone.Common.Application.Contracts.Exceptions;

using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown with an external service does not return an expected response.
/// </summary>
[Serializable]
public class ExternalServiceException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalServiceException"/> with the default message.
    /// </summary>
    public ExternalServiceException() : base("An error occurred while communicating with an external service.")
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalServiceException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ExternalServiceException(string message) : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalServiceException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ExternalServiceException(string message, Exception innerException) : base(message, innerException)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalServiceException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The serialization information.</param>
    /// <param name="context">The streaming context.</param>
    protected ExternalServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
