namespace Waystone.Common.Application.Common.DependencyInjection;

using Contracts.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyMarkers"></param>
    /// <returns></returns>
    public static IWaystoneApplicationBuilder AddWaystoneApplicationBuilder(this IServiceCollection services, params Type[] assemblyMarkers)
    {
        return new WaystoneApplicationBuilder(services, assemblyMarkers);
    }
}
