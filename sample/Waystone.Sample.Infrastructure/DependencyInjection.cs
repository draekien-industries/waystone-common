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
                .AddInMemoryCaching() // or invoke AddRedisCaching when using redis
                .AcceptDefaults();

        services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}
