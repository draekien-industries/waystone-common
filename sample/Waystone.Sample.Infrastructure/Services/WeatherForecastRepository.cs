namespace Waystone.Sample.Infrastructure.Services;

using Application.Features.WeatherForecasts.Contracts;
using Application.Features.WeatherForecasts.Services;
using Common.Application.Contracts.Services;
using Domain.Entities.WeatherForecasts;
using Microsoft.Extensions.Logging;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private static readonly ForecastSummary[] Summaries =
    {
        ForecastSummary.Freezing,
        ForecastSummary.Bracing,
        ForecastSummary.Chilly,
        ForecastSummary.Cool,
        ForecastSummary.Mild,
        ForecastSummary.Warm,
        ForecastSummary.Balmy,
        ForecastSummary.Hot,
        ForecastSummary.Sweltering,
        ForecastSummary.Scorching,
    };

    private readonly IDateTimeProvider _dateTime;
    private readonly ILogger<WeatherForecastRepository> _logger;
    private readonly IRandomProvider _random;
    private readonly IEnumerable<WeatherForecast> _weatherForecasts;

    public WeatherForecastRepository(
        ILogger<WeatherForecastRepository> logger,
        IDateTimeProvider dateTime,
        IRandomProvider random)
    {
        _logger = logger;
        _dateTime = dateTime;
        _random = random;
        _weatherForecasts = GenerateForecasts();
    }

    /// <inheritdoc />
    public IEnumerable<WeatherForecast> Get(int from, int count, ForecastFilterDto? filter)
    {
        _logger.LogInformation("Getting weather forecasts from {From} to {Count}", from, count);

        IQueryable<WeatherForecast> query = ApplyFilter(filter);

        return query.Skip(from).Take(count);
    }

    /// <inheritdoc />
    public int Count(ForecastFilterDto? filter)
    {
        IQueryable<WeatherForecast> query = ApplyFilter(filter);

        return query.Count();
    }

    /// <inheritdoc />
    public bool Any(Guid id)
    {
        return _weatherForecasts.Any(x => x.Id == id);
    }

    /// <inheritdoc />
    public WeatherForecast? Get(Guid id)
    {
        return _weatherForecasts.SingleOrDefault(x => x.Id == id);
    }

    private IEnumerable<WeatherForecast> GenerateForecasts()
    {
        return Enumerable.Range(0, 100)
                         .Select(
                              index => new WeatherForecast(
                                  _dateTime.Now.AddDays(index),
                                  _random.Next(-20, 45),
                                  Summaries[_random.Next(Summaries.Length)]))
                         .ToArray();
    }

    private IQueryable<WeatherForecast> ApplyFilter(ForecastFilterDto? filter)
    {
        IQueryable<WeatherForecast> query = _weatherForecasts.AsQueryable();

        if (filter is null)
        {
            return query;
        }

        if (filter.DesiredSummary is not null)
        {
            query = query.Where(forecast => forecast.Summary == filter.DesiredSummary);
        }

        if (filter.MinimumTemperatureC.HasValue)
        {
            query = query.Where(forecast => forecast.TemperatureC >= filter.MinimumTemperatureC);
        }

        if (filter.MaximumTemperatureC.HasValue)
        {
            query = query.Where(forecast => forecast.TemperatureC <= filter.MaximumTemperatureC);
        }

        return query;
    }
}
