namespace Waystone.Common.Api.Logging;

using ConfigurationOptions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

public static class LogginConfigurationExtensions
{
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
