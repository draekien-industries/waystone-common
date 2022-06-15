namespace Waystone.Sample.Application.Features.WeatherForecasts.Services;

using Contracts;
using Domain.Entities.WeatherForecasts;

/// <summary>The repository providing the weather forecasts.</summary>
public interface IWeatherForecastRepository
{
    /// <summary>Gets the weather forecasts in the specified range.</summary>
    /// <param name="from"></param>
    /// <param name="count"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    IEnumerable<WeatherForecast> Get(int from, int count, ForecastFilterDto? filter);

    /// <summary>Counts the number of weather forecasts that match the requested filter.</summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    int Count(ForecastFilterDto? filter);
}
