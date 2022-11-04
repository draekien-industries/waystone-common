namespace Waystone.Sample.Infrastructure;

using Application.WeatherForecasts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class DependencyInjection
{
    public static IServiceCollection AddSampleInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddWaystoneInfrastructureBuilder(configuration)

                 // .AddRedisCaching() if you want to enable redis
                .AcceptDefaults();

        services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}
