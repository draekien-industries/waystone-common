namespace Waystone.Common.Infrastructure.Services;

using Application.Contracts.Services;

/// <inheritdoc />
public class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.Now;

    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
