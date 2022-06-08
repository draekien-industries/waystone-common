namespace Waystone.Sample.Application.Features.WeatherForecasts.Contracts;

using Common.Application.Contracts.Mappings;
using Domain.Entities.WeatherForecasts;

public class WeatherForecastDto : IMapFrom<WeatherForecast>
{
    public Guid Id { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string Summary { get; set; } = default!;

    public DateTime DateTime { get; set; }
}
