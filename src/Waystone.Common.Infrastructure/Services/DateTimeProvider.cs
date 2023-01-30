namespace Waystone.Common.Infrastructure.Services;

using Application.Contracts.Services;

/// <inheritdoc />
internal sealed class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;

    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime Today => DateTime.Today;
}
