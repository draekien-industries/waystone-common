namespace Waystone.Common.Domain.Contracts.Results;

/// <summary>
/// An error that occured during the normal operation of the application.
/// Used inside the <see cref="Result" /> object.
/// </summary>
/// <param name="Code">The error code.</param>
/// <param name="Message">The error message.</param>
/// <param name="Exception">An optional exception which caused this error.</param>
public record Error(string Code, string Message, Exception? Exception = default);
