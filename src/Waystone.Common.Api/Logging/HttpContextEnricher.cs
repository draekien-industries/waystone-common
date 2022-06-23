namespace Waystone.Common.Api.Logging;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// An enricher that adds http context information to log events.
/// </summary>
internal class HttpContextEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext?.RequestServices.GetService<HttpContextDto>();

        if (httpContext == null) return;

        List<LogEventProperty> properties = new()
        {
            propertyFactory.CreateProperty(nameof(httpContext.UserClaims), httpContext.UserClaims, true),
            propertyFactory.CreateProperty(nameof(httpContext.Host), httpContext.Host.Value, true),
            propertyFactory.CreateProperty(nameof(httpContext.RequestPath), httpContext.RequestPath.Value, true),
            propertyFactory.CreateProperty(nameof(httpContext.RequestHeaders), httpContext.RequestHeaders, true),
            propertyFactory.CreateProperty(nameof(httpContext.QueryStrings), httpContext.QueryStrings, true),
            propertyFactory.CreateProperty(nameof(httpContext.RouteData), httpContext.RouteData, true),
            propertyFactory.CreateProperty(nameof(httpContext.ActionName), httpContext.ActionName),
            propertyFactory.CreateProperty(nameof(httpContext.ControllerName), httpContext.ControllerName),
        };

        foreach (LogEventProperty logEventProperty in properties)
        {
            logEvent.AddOrUpdateProperty(logEventProperty);
        }
    }
}
