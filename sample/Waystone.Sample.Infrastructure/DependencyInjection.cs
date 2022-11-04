namespace Waystone.Sample.Infrastructure;

using Application.WeatherForecasts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

public static class DependencyInjection
{
    public static IServiceCollection AddSampleInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddWaystoneInfrastructureBuilder(configuration)
                .AcceptDefaults(useRedis: environment.IsProduction());

        services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}
