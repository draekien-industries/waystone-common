namespace Waystone.Common.Api.Logging;

using ConfigurationOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog.Core;
using Serilog.Events;

public class CorrelationIdHeaderEnricher : ILogEventEnricher
{
    private const string CorrelationIdPropertyName = "CorrelationId";
    private const string MissingCorrelationIdValue = "None";

    private readonly string _headerName;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdHeaderEnricher(string headerName)
    {
        _headerName = headerName;
        _httpContextAccessor = new HttpContextAccessor();
    }


    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (_httpContextAccessor.HttpContext == null) return;

        string correlationId = GetCorrelationId(_httpContextAccessor.HttpContext);

        LogEventProperty correlationIdProperty = new(CorrelationIdPropertyName, new ScalarValue(correlationId));

        logEvent.AddOrUpdateProperty(correlationIdProperty);
    }

    private string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_headerName, out StringValues correlationId))
        {
            return correlationId.ToString();
        }

        return !string.IsNullOrWhiteSpace(context.TraceIdentifier) ? context.TraceIdentifier : MissingCorrelationIdValue;
    }
}
