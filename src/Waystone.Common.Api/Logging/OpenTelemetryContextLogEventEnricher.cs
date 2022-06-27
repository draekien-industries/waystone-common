namespace Waystone.Common.Api.Logging;

using OpenTelemetry.Trace;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Log event enricher for OpenTelemetry.
/// </summary>
internal class OpenTelemetryContextLogEventEnricher : ILogEventEnricher
{
    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        SpanContext context = Tracer.CurrentSpan.Context;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", context.TraceId.ToHexString()));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", context.SpanId.ToHexString()));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceFlags", context.TraceFlags.ToString()));
    }
}
