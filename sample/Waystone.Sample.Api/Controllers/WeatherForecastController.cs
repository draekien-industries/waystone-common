namespace Waystone.Sample.Api.Controllers;

using Application.WeatherForecasts.Contracts;
using Application.WeatherForecasts.Queries;
using Common.Api.Controllers;
using Common.Application.Contracts.Pagination;
using Microsoft.AspNetCore.Mvc;

/// <summary>Controls the weather forecast resource.</summary>
public class WeatherForecastController : WaystoneApiController
{
    /// <summary>Gets a set of weather forecasts.</summary>
    /// <param name="query"></param>
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

    /// <summary>Gets a weather forecast by it's id.</summary>
    /// <param name="id">The id of the forecast.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="WeatherForecastDto" /></returns>
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(WeatherForecastDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        GetWeatherForecastByIdQuery query = new() { Id = id };

        WeatherForecastDto result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
