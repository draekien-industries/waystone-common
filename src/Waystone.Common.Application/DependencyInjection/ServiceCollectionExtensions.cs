namespace Waystone.Common.Application.DependencyInjection;

using Contracts.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Extensions for the <see cref="IServiceCollection" /> interface.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Creates a new <see cref="IWaystoneApplicationBuilder" /> for configuring dependencies provided by the Waystone
    /// Common Application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblyMarkers">
    /// The assembly markers (types) which will be used to look for mappings, validators, and
    /// request handlers.
    /// </param>
    /// <returns>The application builder.</returns>
    public static IWaystoneApplicationBuilder AddWaystoneApplicationBuilder(
        this IServiceCollection services,
        params Type[] assemblyMarkers)
    {
        return new WaystoneApplicationBuilder(services, assemblyMarkers);
    }
}
