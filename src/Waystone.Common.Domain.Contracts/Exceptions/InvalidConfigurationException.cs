namespace Waystone.Common.Domain.Contracts.Exceptions;

using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when a configuration value is invalid.
/// </summary>
/// <remarks>Reasons for invalid value can be that the value was not found, or the value is not in the expected format.</remarks>
[Serializable]
public class InvalidConfigurationException : Exception
{
    /// <summary>
    /// Creates a new instance of the <see cref="InvalidConfigurationException" /> class.
    /// </summary>
    /// <param name="key">The configuration key that is responsible for the invalid value.</param>
    public InvalidConfigurationException(string key) : base(
        $"Invalid configuration value found. Configuration key: {key}.")
    { }

    /// <summary>
    /// Creates a new instance of the <see cref="InvalidConfigurationException" /> class.
    /// </summary>
    /// <param name="message">The custom exception message.</param>
    /// <param name="key">The configuration key that is responsible for the invalid value.</param>
    public InvalidConfigurationException(string message, string key) : base($"{message}. Configuration key: {key}.")
    { }

    /// <summary>
    /// Creates a new instance of the <see cref="InvalidConfigurationException" /> class.
    /// </summary>
    /// <param name="message">The custom exception message.</param>
    /// <param name="key">The configuration key that is responsible for the invalid value.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public InvalidConfigurationException(string message, string key, Exception innerException) : base(
        $"{message}. Configuration key: {key}.",
        innerException)
    { }


    /// <inheritdoc />
    protected InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
