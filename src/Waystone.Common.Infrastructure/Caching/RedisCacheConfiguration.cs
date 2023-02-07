namespace Waystone.Common.Infrastructure.Caching;

[PublicAPI]
public sealed class RedisCacheConfiguration
{
    public string[] Endpoints { get; init; } = Array.Empty<string>();

    public bool AllowAdmin { get; init; }

    public string? User { get; init; }

    public string? Password { get; init; }

    public string? ClientName { get; init; }

    public int? DefaultDatabase { get; init; }

    public string? ServiceName { get; init; }
}
