namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ServiceCollectionExtensions
{
    public static IWaystoneApiBuilder AddWaystoneApiBuilder(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        return new WaystoneApiBuilder(services, environment, configuration);
    }
}
