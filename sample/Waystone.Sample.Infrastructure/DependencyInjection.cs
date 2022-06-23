namespace Waystone.Sample.Infrastructure;

using Application.WeatherForecasts.Services;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class DependencyInjection
{
    public static IServiceCollection AddSampleInfrastructure(this IServiceCollection services)
    {
        services.AddWaystoneInfrastructureBuilder()
                .AcceptDefaults();

        services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}
