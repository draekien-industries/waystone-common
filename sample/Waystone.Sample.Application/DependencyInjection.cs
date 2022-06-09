namespace Waystone.Sample.Application;

using Common.Application.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddSampleApplication(this IServiceCollection services)
    {
        services.AddWaystoneApplicationBuilder(typeof(DependencyInjection))
                .AcceptDefaults();

        return services;
    }
}
