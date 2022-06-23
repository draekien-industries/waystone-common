namespace Waystone.Common.Api.Middleware;

using Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

/// <summary>
/// Middleware for populating the <see cref="HttpContextDto" /> from the <see cref="HttpContext" />.
/// </summary>
internal class HttpContextDtoMiddleware
{
    private readonly RequestDelegate _next;

    public HttpContextDtoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context, HttpContextDto dto)
    {
        dto.UserClaims = context.User.Claims;
        dto.Host = context.Request.Host;
        dto.RequestPath = context.Request.Path;
        dto.RequestHeaders = context.Request.Headers.ToDictionary(header => header.Key, header => header.Value);
        dto.QueryStrings = QueryHelpers.ParseQuery(context.Request.QueryString.ToString())
                                       .ToDictionary(query => query.Key, query => query.Value);

        return _next(context);
    }
}
