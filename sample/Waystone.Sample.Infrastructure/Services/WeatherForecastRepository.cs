namespace Waystone.Sample.Infrastructure.Services;

using Application.Features.WeatherForecasts.Services;
using Domain.Entities.WeatherForecasts;
using Microsoft.Extensions.Logging;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastRepository> _logger;

    public WeatherForecastRepository(ILogger<WeatherForecastRepository> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public IEnumerable<WeatherForecast> Get(int from, int count)
    {
        _logger.LogInformation("Getting weather forecasts from {From} to {Count}", from, count);

        return Enumerable.Range(from, count)
                         .Select(
                              index => new WeatherForecast(
                                  DateTime.Now.AddDays(index),
                                  Random.Shared.Next(-20, 45),
                                  Summaries[Random.Shared.Next(Summaries.Length)]))
                         .ToArray();
    }
}
