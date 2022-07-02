// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using Configuration;

/// <summary>Extensions for the <see cref="IServiceCollection" /> interface.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="IWaystoneInfrastructureBuilder" /> for configuring dependencies provided by the
    /// Waystone Common Infrastructure.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The infrastructure builder.</returns>
    public static IWaystoneInfrastructureBuilder AddWaystoneInfrastructureBuilder(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return new WaystoneInfrastructureBuilder(services, configuration);
    }
}
