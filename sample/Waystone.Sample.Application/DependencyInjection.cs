namespace Waystone.Sample.Application;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Dependency injection for the sample application.</summary>
public static class DependencyInjection
{
    /// <summary>Adds the services used by the sample application.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app's configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddSampleApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddWaystoneApplicationBuilder(configuration, typeof(DependencyInjection))
                .AddCachingPipelineBehaviour()
                .AcceptDefaults();

        return services;
    }
}
