namespace Waystone.Sample.Application.Features.WeatherForecasts.Contracts;

using Common.Application.Contracts.Mappings;
using Domain.Entities.WeatherForecasts;

/// <summary>A weather forecast.</summary>
public class WeatherForecastDto : IMapFrom<WeatherForecast>
{
    /// <summary>The id of the forecast.</summary>
    public Guid Id { get; set; }

    /// <summary>The temperature in Celsius.</summary>
    public int TemperatureC { get; set; }

    /// <summary>The temperature in Fahrenheit.</summary>
    public int TemperatureF { get; set; }

    /// <summary>The summary of the forecast.</summary>
    public ForecastSummaries Summary { get; set; } = default!;

    /// <summary>The date of the forecast.</summary>
    public DateTime DateTime { get; set; }
}
