// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>Extensions for the <see cref="IServiceCollection" /> interface.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="IWaystoneInfrastructureBuilder" /> for configuring dependencies provided by the
    /// Waystone Common Infrastructure.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The infrastructure builder.</returns>
    public static IWaystoneInfrastructureBuilder AddWaystoneInfrastructureBuilder(this IServiceCollection services)
    {
        return new WaystoneInfrastructureBuilder(services);
    }
}
