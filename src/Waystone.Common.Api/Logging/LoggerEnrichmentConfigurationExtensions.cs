namespace Waystone.Common.Api.Logging;

using ConfigurationOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;

/// <summary>
/// Extensions for the <see cref="LoggerEnrichmentConfiguration" />.
/// </summary>
public static class LoggerEnrichmentConfigurationExtensions
{
    /// <summary>
    /// Registers the <see cref="CorrelationIdHeaderEnricher" /> as a logger enricher.
    /// </summary>
    /// <param name="loggerConfig">The <see cref="LoggerEnrichmentConfiguration" />.</param>
    /// <param name="appConfig">The <see cref="IConfiguration" />.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider" />.</param>
    /// <returns>The <see cref="LoggerConfiguration" />.</returns>
    public static LoggerConfiguration WithCorrelationIdHeader(
        this LoggerEnrichmentConfiguration loggerConfig,
        IConfiguration appConfig,
        IServiceProvider serviceProvider)
    {
        var config = appConfig.GetSection(CorrelationIdHeaderOptions.SectionName)
                              .Get<CorrelationIdHeaderOptions>();

        string headerName = config?.HeaderName ?? CorrelationIdHeaderOptions.DefaultHeaderName;
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        CorrelationIdHeaderEnricher enricher = new(headerName, httpContextAccessor);

        return loggerConfig.With(enricher);
    }

    /// <summary>
    /// Registers the <see cref="HttpContextEnricher" /> as a logger enricher.
    /// </summary>
    /// <param name="loggerConfig">The <see cref="LoggerEnrichmentConfiguration" />.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider" />.</param>
    /// <returns>The <see cref="LoggerConfiguration" />.</returns>
    public static LoggerConfiguration WithHttpContext(
        this LoggerEnrichmentConfiguration loggerConfig,
        IServiceProvider serviceProvider)
    {
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        HttpContextEnricher enricher = new(httpContextAccessor);

        return loggerConfig.With(enricher);
    }

    /// <summary>
    /// Registers the <see cref="OpenTelemetryContextLogEventEnricher" />.
    /// </summary>
    /// <param name="config">The <see cref="LoggerEnrichmentConfiguration" />.</param>
    /// <returns>The <see cref="LoggerConfiguration" />.</returns>
    public static LoggerConfiguration WithOpenTelemetryContext(this LoggerEnrichmentConfiguration config)
    {
        return config.With(new OpenTelemetryContextLogEventEnricher());
    }
}
