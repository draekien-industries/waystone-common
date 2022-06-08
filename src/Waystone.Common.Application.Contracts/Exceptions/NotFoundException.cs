namespace Waystone.Common.Application.Contracts.Exceptions;

using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when the requested resource does not exist.
/// </summary>
[Serializable]
public class NotFoundException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> with the default error message.
    /// </summary>
    public NotFoundException() : base("The requested resource could not be found.")
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> using an entity type and the key that was used to access the resource.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <param name="key">The key used to access the resource.</param>
    public NotFoundException(Type entityType, string key) : base($"The requested resource '{entityType.Name}' ({key}) could not be found.")
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> using the specified error message.
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string message) : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> using the specified error message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> using the specified serialization information and context.
    /// </summary>
    /// <param name="info">The serialization information.</param>
    /// <param name="context">The streaming context.</param>
    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
