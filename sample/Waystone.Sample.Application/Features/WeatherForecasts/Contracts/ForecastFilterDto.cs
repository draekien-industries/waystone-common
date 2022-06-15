namespace Waystone.Sample.Application.Features.WeatherForecasts.Contracts;

using Common.Application.Contracts.Mappings;
using Domain.Entities.WeatherForecasts;
using Queries;

public class ForecastFilterDto : IMapFrom<GetWeatherForecastsQuery>
{
    public ForecastSummary? DesiredSummary { get; init; }

    public int? MinimumTemperatureC { get; init; }

    public int? MaximumTemperatureC { get; init; }
}
