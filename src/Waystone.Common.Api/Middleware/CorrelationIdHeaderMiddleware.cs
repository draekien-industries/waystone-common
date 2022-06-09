namespace Waystone.Common.Api.Middleware;

using ConfigurationOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

/// <summary>Middleware to handle the correlation ID header.</summary>
public class CorrelationIdHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationIdHeaderOptions _options;

    public CorrelationIdHeaderMiddleware(RequestDelegate next, IOptions<CorrelationIdHeaderOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public Task Invoke(HttpContext context)
    {
        context.TraceIdentifier = GetCorrelationId(context);

        if (_options.IncludeInResponse)
        {
            context.Response.OnStarting(
                () =>
                {
                    context.Response.Headers.TryAdd(_options.HeaderName, new StringValues(context.TraceIdentifier));

                    return Task.CompletedTask;
                });
        }

        return _next(context);
    }

    private string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(_options.HeaderName, out StringValues correlationId))
        {
            return correlationId.ToString();
        }

        var generatedCorrelationId = Guid.NewGuid().ToString();
        StringValues correlationIdHeaderValue = new(generatedCorrelationId);
        context.Request.Headers.TryAdd(_options.HeaderName, correlationIdHeaderValue);

        return generatedCorrelationId;
    }
}
