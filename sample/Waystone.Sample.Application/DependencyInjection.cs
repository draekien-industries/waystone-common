namespace Waystone.Sample.Application;

using Common.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Dependency injection for the sample application.</summary>
public static class DependencyInjection
{
    /// <summary>Adds the services used by the sample application.</summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddSampleApplication(this IServiceCollection services)
    {
        services.AddWaystoneApplicationBuilder(typeof(DependencyInjection))
                .AcceptDefaults();

        return services;
    }
}
