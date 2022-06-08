namespace Waystone.Sample.Api.Controllers;

using Application.Features.WeatherForecasts.Contracts;
using Application.Features.WeatherForecasts.Queries;
using Microsoft.AspNetCore.Mvc;
using Common.Api.Controllers;
using Common.Application.Contracts.Pagination;

public class WeatherForecastController : WaystoneApiController
{
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
