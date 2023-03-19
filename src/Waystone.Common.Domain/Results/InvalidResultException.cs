namespace Waystone.Common.Domain.Results;

using System.Runtime.Serialization;

/// <summary>
/// Exception for an invalid <see cref="Result{TValue}" />
/// </summary>
[Serializable]
public class InvalidResultException : Exception
{
    /// <summary>
    /// Creates a new instance of the exception with a default message.
    /// </summary>
    public InvalidResultException() : base("The result is not in a valid state.")
    { }

    /// <summary>
    /// Creates a new instance of the exception with a specified message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public InvalidResultException(string message) : base(message)
    { }

    /// <summary>
    /// Creates a new instance of the exception with a specified message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public InvalidResultException(string message, Exception innerException) : base(message, innerException)
    { }

    /// <inheritdoc />
    protected InvalidResultException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
