namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>Extensions for the <see cref="IServiceCollection" />.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Creates a new <see cref="IWaystoneApiBuilder" /> for configuring dependencies provided by the Waystone Common
    ///     Api.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="environment">The host environment.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The <see cref="IWaystoneApiBuilder" />.</returns>
    public static IWaystoneApiBuilder AddWaystoneApiBuilder(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        return new WaystoneApiBuilder(services, environment, configuration);
    }
}
