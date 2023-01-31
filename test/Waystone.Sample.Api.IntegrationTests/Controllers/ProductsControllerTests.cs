namespace Waystone.Sample.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using Application.Products.CreateProduct;

public sealed class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IFixture _fixture;

    public ProductsControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _fixture = new Fixture();
        _factory = factory;
    }

    [Fact]
    public async Task GivenValidCreateProductCommand_WhenCreatingProduct_ThenReturnCreatedResponse()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        CreateProductCommand? command = _fixture.Build<CreateProductCommand>()
                                                .With(x => x.AmountExcludingTax, 100)
                                                .With(x => x.DiscountPercentage, 0.1m)
                                                .With(x => x.TaxPercentage, 0.1m)
                                                .Create();

        Uri uri = new("Products", UriKind.Relative);

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, command);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenInvalidCreateProductCommand_WhenCreatingProduct_ThenReturnUnprocessableEntity()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        CreateProductCommand? command = _fixture.Build<CreateProductCommand>()
                                                .With(x => x.AmountExcludingTax, -100)
                                                .With(x => x.DiscountPercentage, -0.1m)
                                                .With(x => x.TaxPercentage, -0.1m)
                                                .Create();

        Uri uri = new("Products", UriKind.Relative);

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
