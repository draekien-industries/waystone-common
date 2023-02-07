namespace Waystone.Common.Api.Logging;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Contract for logging the HttpContext.
/// </summary>
[PublicAPI]
public sealed class HttpContextDto
{
    /// <summary>
    /// The <see cref="Claim" />s of the current user.
    /// </summary>
    public IEnumerable<Claim>? UserClaims { get; internal set; }

    /// <summary>
    /// The host the request originated from.
    /// </summary>
    public HostString Host { get; internal set; }

    /// <summary>
    /// The headers from the request.
    /// </summary>
    public Dictionary<string, StringValues> RequestHeaders { get; internal set; } = new();

    /// <summary>
    /// The query strings from the request.
    /// </summary>
    public Dictionary<string, StringValues> QueryStrings { get; internal set; } = new();

    /// <summary>
    /// The request path from the request.
    /// </summary>
    public PathString RequestPath { get; internal set; }

    /// <summary>
    /// The route data from the request.
    /// </summary>
    public Dictionary<string, object?> RouteData { get; internal set; } = new();

    /// <summary>
    /// The name of the controller executing the request.
    /// </summary>
    public string? ControllerName { get; internal set; }

    /// <summary>
    /// The name of the action executing the request.
    /// </summary>
    public string? ActionName { get; internal set; }
}
