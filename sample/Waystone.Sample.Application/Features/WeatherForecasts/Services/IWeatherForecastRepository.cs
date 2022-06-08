namespace Waystone.Sample.Application.Features.WeatherForecasts.Services;

using Domain.Entities.WeatherForecasts;

public interface IWeatherForecastRepository
{
    IEnumerable<WeatherForecast> Get(int from, int count);
}
