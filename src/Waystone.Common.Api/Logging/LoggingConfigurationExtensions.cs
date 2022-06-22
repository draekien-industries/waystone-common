namespace Waystone.Common.Api.Logging;

using ConfigurationOptions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

/// <summary>Extensions for the <see cref="LoggerEnrichmentConfiguration" />.</summary>
public static class LoggingConfigurationExtensions
{
    /// <summary>Registers the <see cref="CorrelationIdHeaderEnricher" /> as a logger enricher.</summary>
    /// <param name="loggerConfig">The <see cref="LoggerEnrichmentConfiguration" />.</param>
    /// <param name="appConfig">The <see cref="IConfiguration" />.</param>
    /// <returns>The <see cref="LoggerConfiguration" />.</returns>
    public static LoggerConfiguration WithCorrelationIdHeader(
        this LoggerEnrichmentConfiguration loggerConfig,
        IConfiguration appConfig)
    {
        var config = appConfig.GetSection(CorrelationIdHeaderOptions.SectionName)
                              .Get<CorrelationIdHeaderOptions>();

        string headerName = config?.HeaderName ?? CorrelationIdHeaderOptions.DefaultHeaderName;
        CorrelationIdHeaderEnricher enricher = new(headerName);

        return loggerConfig.With(enricher);
    }
}
