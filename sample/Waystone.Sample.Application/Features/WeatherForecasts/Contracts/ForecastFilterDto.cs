namespace Waystone.Sample.Application.Features.WeatherForecasts.Contracts;

using Common.Application.Contracts.Mappings;
using Domain.Entities.WeatherForecasts;
using Queries;

/// <summary>The filter for the weather forecast query.</summary>
public class ForecastFilterDto : IMapFrom<GetWeatherForecastsQuery>
{
    /// <summary>The desired <see cref="ForecastSummary" /></summary>
    public ForecastSummary? DesiredSummary { get; init; }

    /// <summary>The minimum temperature cutoff for the results.</summary>
    public int? MinimumTemperatureC { get; init; }

    /// <summary>The maximum temperature cutoff for the results.</summary>
    public int? MaximumTemperatureC { get; init; }
}
