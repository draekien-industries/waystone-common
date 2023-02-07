namespace Waystone.Common.Application.Contracts.Caching;

/// <summary>
/// Options for the default caching behaviour.
/// </summary>
[PublicAPI]
public sealed class DefaultCacheOptions
{
    /// <summary>
    /// The default expiry seconds of a cache entry. It is used to set the
    /// absolute expiry time relative to now.
    /// </summary>
    public int ExpirySeconds { get; init; } = 300;
}
