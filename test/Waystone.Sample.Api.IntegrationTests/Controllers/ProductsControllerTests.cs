namespace Waystone.Sample.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using Application.Products;
using Application.Products.CreateProduct;
using Common.Application.Contracts.Pagination;

public sealed class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly List<Guid> _createdProducts;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IFixture _fixture;

    public ProductsControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _fixture = new Fixture();
        _createdProducts = new List<Guid>();
        _factory = factory;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        HttpClient client = _factory.CreateClient();
        IEnumerable<CreateProductCommand>? createProductCommands = _fixture.Build<CreateProductCommand>()
                                                                           .With(x => x.AmountExcludingTax, 100)
                                                                           .With(x => x.DiscountPercentage, 0.1m)
                                                                           .With(x => x.TaxPercentage, 0.1m)
                                                                           .CreateMany(30);

        Uri createUri = new("Products", UriKind.Relative);

        foreach (CreateProductCommand? command in createProductCommands)
        {
            HttpResponseMessage createResponse = await client.PostAsJsonAsync(createUri, command);
            createResponse.EnsureSuccessStatusCode();
            var product = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
            _createdProducts.Add(product!.Id);
        }
    }

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
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

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();

        _createdProducts.Add(product!.Id);
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

    [Fact]
    public async Task GivenExistingProduct_WhenGettingProductById_ThenReturnTheProduct()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        CreateProductCommand? createProductCommand = _fixture.Build<CreateProductCommand>()
                                                             .With(x => x.AmountExcludingTax, 100)
                                                             .With(x => x.DiscountPercentage, 0.1m)
                                                             .With(x => x.TaxPercentage, 0.1m)
                                                             .Create();

        Uri createUri = new("Products", UriKind.Relative);
        HttpResponseMessage createResponse = await client.PostAsJsonAsync(createUri, createProductCommand);
        createResponse.EnsureSuccessStatusCode();

        var expected = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        _createdProducts.Add(expected!.Id);

        // Act
        var actual = await client.GetFromJsonAsync<ProductDto>(createResponse.Headers.Location);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GivenNoMatchingProduct_WhenGettingProductById_ThenReturnNotFound()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Uri uri = new($"Products/{Guid.NewGuid()}", UriKind.Relative);

        // Act
        HttpResponseMessage response = await client.GetAsync(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenExistingProduct_WhenDeletingProductById_ThenReturnNoContent()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        // Act
        HttpResponseMessage deleteResponse =
            await client.DeleteAsync(new Uri($"/products/{_createdProducts.Last()}", UriKind.Relative));

        // Assert
        deleteResponse.EnsureSuccessStatusCode();
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GivenNoMatchingProduct_WhenDeletingProductById_ThenReturnNoContent()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        Uri uri = new($"Products/{Guid.NewGuid()}", UriKind.Relative);

        // Act
        HttpResponseMessage response = await client.DeleteAsync(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task WhenListingProducts_ThenReturnPaginationLinks()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        Uri listUri = new("Products", UriKind.Relative);

        // Act
        var actual = await client.GetFromJsonAsync<PaginatedResponse<ProductDto>>(listUri);

        // Assert
        actual.Should().NotBeNull();
        actual!.Links.Should().NotBeNull();
        actual.Links!.Self.Should().BeEquivalentTo(new Uri("/products?limit=10&cursor=0", UriKind.Relative));
        actual.Links.Next.Should().BeEquivalentTo(new Uri("/products?limit=10&cursor=10", UriKind.Relative));
        actual.Links.Previous.Should().BeNull();
    }

    [Fact]
    public async Task GivenMoreThanOnePageOfProducts_WhenListingProductsOnPage2_ThenReturnCorrectPaginationLinks()
    {
        // Arrange
        HttpClient client = _factory.CreateClient();

        Uri listUri = new("/products?limit=15&cursor=15", UriKind.Relative);

        // Act
        var actual = await client.GetFromJsonAsync<PaginatedResponse<ProductDto>>(listUri);

        // Assert
        actual.Should().NotBeNull();
        actual!.Links.Should().NotBeNull();

        actual.Links!.Self.Should().BeEquivalentTo(new Uri("/products?limit=15&cursor=15", UriKind.Relative));
        actual.Links.Previous.Should().BeEquivalentTo(new Uri("/products?limit=15&cursor=0", UriKind.Relative));
        actual.Links.Next.Should().BeEquivalentTo(new Uri("/products?limit=15&cursor=30", UriKind.Relative));
    }
}
