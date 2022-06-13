namespace Waystone.Sample.Infrastructure.Services;

using Application.Features.WeatherForecasts.Services;
using Common.Application.Contracts.Services;
using Domain.Entities.WeatherForecasts;
using Microsoft.Extensions.Logging;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly ForecastSummaries[] Summaries =
    {
        ForecastSummaries.Freezing,
        ForecastSummaries.Bracing,
        ForecastSummaries.Chilly,
        ForecastSummaries.Cool,
        ForecastSummaries.Mild,
        ForecastSummaries.Warm,
        ForecastSummaries.Balmy,
        ForecastSummaries.Hot,
        ForecastSummaries.Sweltering,
        ForecastSummaries.Scorching,
    };

    private readonly IDateTimeProvider _dateTime;
    private readonly ILogger<WeatherForecastRepository> _logger;
    private readonly IRandomProvider _random;

    public WeatherForecastRepository(
        ILogger<WeatherForecastRepository> logger,
        IDateTimeProvider dateTime,
        IRandomProvider random)
    {
        _logger = logger;
        _dateTime = dateTime;
        _random = random;
    }

    /// <inheritdoc />
    public IEnumerable<WeatherForecast> Get(int from, int count)
    {
        _logger.LogInformation("Getting weather forecasts from {From} to {Count}", from, count);

        return Enumerable.Range(from, count)
                         .Select(
                              index => new WeatherForecast(
                                  _dateTime.Now.AddDays(index),
                                  _random.Next(-20, 45),
                                  Summaries[_random.Next(Summaries.Length)]))
                         .ToArray();
    }
}
