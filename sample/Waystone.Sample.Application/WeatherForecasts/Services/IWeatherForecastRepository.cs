namespace Waystone.Sample.Application.WeatherForecasts.Services;

using Common.Domain.Contracts.Results;
using Contracts;
using Domain.Entities.WeatherForecasts;

/// <summary>The repository providing the weather forecasts.</summary>
public interface IWeatherForecastRepository
{
    /// <summary>Gets the weather forecast with the specified id.</summary>
    /// <param name="id">The id of the forecast.</param>
    /// <returns>The weather forecast.</returns>
    Result<WeatherForecast> Get(Guid id);

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

    /// <summary>Checks to see if the weather forecast with the specified id exists.</summary>
    /// <param name="id">The id of the forecast.</param>
    /// <returns>True when the id exists.</returns>
    bool Any(Guid id);
}
