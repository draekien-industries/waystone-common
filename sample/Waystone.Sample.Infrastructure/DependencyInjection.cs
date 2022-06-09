namespace Waystone.Sample.Infrastructure;

using Application.Features.WeatherForecasts.Services;
using Common.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class DependencyInjection
{
    public static IServiceCollection AddSampleInfrastructure(this IServiceCollection services)
    {
        services.AddWaystoneInfrastructureBuilder()
                .AcceptDefaults();

        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}
