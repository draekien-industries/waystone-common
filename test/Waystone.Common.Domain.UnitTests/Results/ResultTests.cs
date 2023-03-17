namespace Waystone.Common.Domain.UnitTests.Results;

using Domain.Results;
using Sample.Domain.Prices;
using Sample.Domain.Products;

public class ResultTests
{
    [Fact]
    public void GivenValidName_WhenUpdatingProductName_ThenResultShouldBeSuccess()
    {
        // Arrange
        Result<Price> createPriceResult = Price.Create(1, 0.1m);
        Result<Product> createProductResult = Product.CreateTransient("abc", createPriceResult.Value, "abc");

        Product product = createProductResult.Value;

        // Act
        Result updateNameResult = product.UpdateName("efg");

        // Assert
        updateNameResult.Succeeded.Should().BeTrue();
        product.Name.Should().Be("efg");
    }

    [Fact]
    public void GivenEmptyName_WhenUpdatingProductName_ThenResultShouldBeFail()
    {
        // Arrange
        Result<Price> createPriceResult = Price.Create(1, 0.1m);
        Result<Product> createProductResult = Product.CreateTransient("abc", createPriceResult.Value, "abc");

        Product product = createProductResult.Value;

        // Act
        Result updateNameResult = product.UpdateName("");

        // Assert
        updateNameResult.Succeeded.Should().BeFalse();
        updateNameResult.Errors.Should().Contain(ProductErrors.UnNamed);
        product.Name.Should().Be("abc");
    }
}
