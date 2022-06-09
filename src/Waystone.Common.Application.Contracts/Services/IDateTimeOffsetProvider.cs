namespace Waystone.Common.Application.Contracts.Services;

/// <summary>The date time offset provider.</summary>
public interface IDateTimeOffsetProvider
{
    /// <summary>Returns the current date time offset representing the current time in the local time zone.</summary>
    DateTimeOffset Now { get; }

    /// <summary>Returns the current date time offset representing the current time in the UTC time zone.</summary>
    DateTimeOffset UtcNow { get; }
}
