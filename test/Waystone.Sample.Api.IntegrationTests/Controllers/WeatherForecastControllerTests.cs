namespace Waystone.Sample.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Mime;
using Application.Features.WeatherForecasts.Queries;
using Microsoft.AspNetCore.Http;

public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task WhenInvokingGetWeatherForecast_ThenReturnSuccessAndCorrectContentType()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        GetWeatherForecastsQuery query = new()
        {
            Cursor = 0,
            Limit = 10,
        };

        var queryString = QueryString.Create(
            new KeyValuePair<string, string?>[]
            {
                new(nameof(query.Cursor), query.Cursor.ToString()),
                new(nameof(query.Limit), query.Limit.ToString()),
            });

        Uri uri = new($"weatherforecast{queryString}", UriKind.Relative);

        // Act
        HttpResponseMessage response = await client.GetAsync(uri);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Theory]
    [InlineData("Random", null, 100)]
    public async Task GivenInvalidFilters_WhenInvokingGetWeahterForecast_ThenReturnBadRequest(
        string? desiredSummary,
        int? minimumTemperatureC,
        int? maximumTemperatureC)
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Dictionary<string, string> queryParameters = new();

        if (desiredSummary != null)
        {
            queryParameters.Add(nameof(desiredSummary), desiredSummary);
        }

        if (minimumTemperatureC != null)
        {
            queryParameters.Add(nameof(minimumTemperatureC), minimumTemperatureC.Value.ToString());
        }

        if (maximumTemperatureC != null)
        {
            queryParameters.Add(nameof(maximumTemperatureC), maximumTemperatureC.Value.ToString());
        }

        var queryString = QueryString.Create(queryParameters!);

        Uri uri = new($"weatherforecast{queryString}", UriKind.Relative);

        // Act
        HttpResponseMessage response = await client.GetAsync(uri);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
