namespace Waystone.Sample.Application.Features.WeatherForecasts.Services;

using Domain.Entities.WeatherForecasts;

/// <summary>The repository providing the weather forecasts.</summary>
public interface IWeatherForecastRepository
{
    /// <summary>Gets the weather forecasts in the specified range.</summary>
    /// <param name="from"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    IEnumerable<WeatherForecast> Get(int from, int count);
}
