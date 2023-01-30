// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;
using Configuration;
using Hosting;
using Serilog;
using Waystone.Common.Api.Logging;

/// <summary>
/// Extensions for the <see cref="IWaystoneApiHostBuilder" />.
/// </summary>
[PublicAPI]
public static class WaystoneApiHostBuilderExtensions
{
    /// <summary>
    /// Accepts the default configuration provided by the <see cref="IWaystoneApiHostBuilder" />.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApiHostBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiHostBuilder" />.</returns>
    public static ConfigureHostBuilder AcceptDefaults(this IWaystoneApiHostBuilder builder)
    {
        builder.UseWaystoneSerilogConfiguration();

        return builder.Host;
    }

    /// <summary>
    /// Configures the host to use the waystone api serilog configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApiHostBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiHostBuilder" />.</returns>
    public static IWaystoneApiHostBuilder UseWaystoneSerilogConfiguration(this IWaystoneApiHostBuilder builder)
    {
        builder.Host.UseSerilog(ConfigureSerilog);

        return builder;
    }

    private static void ConfigureSerilog(
        HostBuilderContext hostBuilderContext,
        IServiceProvider serviceProvider,
        LoggerConfiguration loggerConfiguration)
    {
        IConfiguration? configuration = hostBuilderContext.Configuration;

        loggerConfiguration.ReadFrom.Configuration(configuration);
        loggerConfiguration.Enrich.WithCorrelationIdHeader(configuration, serviceProvider);
        loggerConfiguration.Enrich.WithHttpContext(serviceProvider);
        loggerConfiguration.Enrich.WithOpenTelemetryContext();
    }
}
