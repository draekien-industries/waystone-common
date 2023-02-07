// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using Configuration;
using Hosting;

/// <summary>Extensions for the <see cref="IServiceCollection" />.</summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="IWaystoneApiServiceBuilder" /> for configuring dependencies provided by the Waystone Common
    /// Api.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="environment">The host environment.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblyMarkers"></param>
    /// <returns>The <see cref="IWaystoneApiServiceBuilder" />.</returns>
    public static IWaystoneApiServiceBuilder AddWaystoneApiServiceBuilder(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration,
        params Type[] assemblyMarkers)
    {
        return new WaystoneApiServiceBuilder(
            services,
            environment,
            configuration,
            assemblyMarkers.Select(marker => marker.Assembly).ToList());
    }
}
