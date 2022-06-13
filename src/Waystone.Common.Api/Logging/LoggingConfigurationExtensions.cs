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
        IConfigurationSection? correlationIdHeaderConfig = appConfig.GetSection(CorrelationIdHeaderOptions.SectionName);
        var headerName = correlationIdHeaderConfig?.GetValue<string>(nameof(CorrelationIdHeaderOptions.HeaderName));

        return loggerConfig.With(
            new CorrelationIdHeaderEnricher(headerName ?? CorrelationIdHeaderOptions.DefaultHeaderName));
    }
}
