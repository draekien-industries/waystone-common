namespace Waystone.Sample.Api.Controllers;

using Application.Features.WeatherForecasts.Contracts;
using Application.Features.WeatherForecasts.Queries;
using Common.Api.Controllers;
using Common.Application.Contracts.Pagination;
using Microsoft.AspNetCore.Mvc;

/// <summary>Controls the weather forecast resource.</summary>
public class WeatherForecastController : WaystoneApiController
{
    /// <summary>Gets a set of weather forecasts.</summary>
    /// <param name="query">The <see cref="GetWeatherForecastsQuery" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The paginated list of <see cref="WeatherForecastDto" />.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<WeatherForecastDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(
        [FromQuery] GetWeatherForecastsQuery query,
        CancellationToken cancellationToken)
    {
        PaginatedResponse<WeatherForecastDto> result = await Mediator.Send(query, cancellationToken);

        result.Links = CreatePaginationLinks(nameof(GetAsync), query, result);

        return Ok(result);
    }
}
