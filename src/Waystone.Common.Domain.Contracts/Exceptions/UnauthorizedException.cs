namespace Waystone.Common.Domain.Contracts.Exceptions;

using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when a user attempts to perform an action that requires authenticated access, but
/// the user is not authenticated.
/// </summary>
[Serializable]
public class UnauthorizedException : ApplicationException
{
    /// <summary>Initializes a new instance of the <see cref="UnauthorizedException" /> with the default message.</summary>
    public UnauthorizedException() : base("Access to the requested resource requires authentication.")
    { }

    /// <summary>Initializes a new instance of the <see cref="UnauthorizedException" /> with the specified message.</summary>
    /// <param name="message">The exception message.</param>
    public UnauthorizedException(string message) : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException" /> with the specified message and inner
    /// exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    { }

    /// <summary>Initializes a new instance of the <see cref="UnauthorizedException" /> with serialized data.</summary>
    /// <param name="info">The serialization information.</param>
    /// <param name="context">The streaming context.</param>
    protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
