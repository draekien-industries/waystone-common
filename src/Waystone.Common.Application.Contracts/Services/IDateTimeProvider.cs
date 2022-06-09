namespace Waystone.Common.Application.Contracts.Services;

/// <summary>The date time provider.</summary>
public interface IDateTimeProvider
{
    /// <summary>Returns a DateTime representing the current date and time.</summary>
    DateTime Now { get; }

    /// <summary>Returns a DateTime representing the current UTC date and time.</summary>
    DateTime UtcNow { get; }

    /// <summary>
    ///     Returns a DateTime representing the current date. The date part of the returned value is the current date, and
    ///     the time part of the returned value is zero (midnight).
    /// </summary>
    DateTime Today { get; }
}
