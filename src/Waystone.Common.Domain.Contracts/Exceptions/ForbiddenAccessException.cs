namespace Waystone.Common.Domain.Contracts.Exceptions;

using System.Runtime.Serialization;

/// <summary>An exception that is thrown when an action is forbidden by the application.</summary>
[Serializable]
public class ForbiddenAccessException : ApplicationException
{
    /// <summary>Initializes a new instance of the <see cref="ForbiddenAccessException" /> class with a default message.</summary>
    public ForbiddenAccessException() : base("Access to the requested resource is denied.")
    { }

    /// <summary>Initializes a new instance of the <see cref="ForbiddenAccessException" /> class with a specified message.</summary>
    /// <param name="message">The exception message.</param>
    public ForbiddenAccessException(string message) : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException" /> class with a specified message and
    /// inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException)
    { }

    /// <summary>Initializes a new instance of the <see cref="ForbiddenAccessException" /> class with serialized data.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    protected ForbiddenAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
