namespace Waystone.Common.Api.Logging;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Core;
using Serilog.Events;

/// <summary>Enriches the log event with the correlation id header.</summary>
internal sealed class CorrelationIdHeaderEnricher : ILogEventEnricher
{
    private const string CorrelationIdPropertyName = "CorrelationId";
    private const string MissingCorrelationIdValue = "None";

    private readonly string _headerName;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>Creates a new instance of the <see cref="CorrelationIdHeaderEnricher" /> class.</summary>
    /// <param name="headerName"></param>
    /// <param name="httpContextAccessor"></param>
    public CorrelationIdHeaderEnricher(string headerName, IHttpContextAccessor httpContextAccessor)
    {
        _headerName = headerName;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (_httpContextAccessor.HttpContext == null) return;

        string correlationId = GetCorrelationId(_httpContextAccessor.HttpContext);

        LogEventProperty correlationIdProperty =
            propertyFactory.CreateProperty(CorrelationIdPropertyName, correlationId);

        logEvent.AddOrUpdateProperty(correlationIdProperty);
    }

    private string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_headerName, out StringValues correlationId))
        {
            return correlationId.ToString();
        }

        return !string.IsNullOrWhiteSpace(context.TraceIdentifier)
            ? context.TraceIdentifier
            : MissingCorrelationIdValue;
    }
}
