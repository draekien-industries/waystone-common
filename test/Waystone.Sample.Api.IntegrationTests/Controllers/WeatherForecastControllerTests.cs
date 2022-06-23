namespace Waystone.Sample.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Mime;
using Application.WeatherForecasts.Contracts;
using Application.WeatherForecasts.Queries;
using Common.Application.Contracts.Pagination;
using Common.Application.Extensions;
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

        var content = await response.Content.DeserializeObjectAsync<PaginatedResponse<WeatherForecastDto>>();

        content.Should().NotBeNull();
        content!.Results.Should().HaveCount(query.Limit);
        content.Links.Should().NotBeNull();
        content.Links!.Previous.Should().BeNull();
        content.Links.Self.Should().NotBeNull();
        content.Links.Next.Should().NotBeNull();
        content.Total.Should().Be(100);
    }

    [Fact]
    public async Task
        GivenCursorIsWithinLimitOfForecastCount_WhenInvokingGetWeatherForecast_ThenLinksShouldNotContainNext()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        GetWeatherForecastsQuery query = new()
        {
            Cursor = 90,
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

        var content = await response.Content.DeserializeObjectAsync<PaginatedResponse<WeatherForecastDto>>();

        content.Should().NotBeNull();
        content!.Results.Should().HaveCount(query.Limit);
        content.Links.Should().NotBeNull();
        content.Links!.Previous.Should().NotBeNull();
        content.Links.Self.Should().NotBeNull();
        content.Links.Next.Should().BeNull();
        content.Total.Should().Be(100);
    }

    [Theory]
    [InlineData("Random", null, 100)]
    [InlineData("Random", 0, null)]
    [InlineData("Scorching", 10, 0)]
    public async Task GivenInvalidFilters_WhenInvokingGetWeatherForecast_ThenReturnBadRequest(
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
