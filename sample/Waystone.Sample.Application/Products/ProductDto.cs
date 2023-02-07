namespace Waystone.Sample.Application.Products;

using Domain.Products;

/// <summary>
/// A public facing representation of the <see cref="Product" /> entity.
/// </summary>
public sealed class ProductDto
{
    /// <summary>
    /// The product's ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The product's description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The product's price (excluding tax).
    /// </summary>
    public decimal AmountExcludingTax { get; set; }

    /// <summary>
    /// The amount of tax charged for this product.
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// The total amount charged for this product.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Creates a new instance of this DTO from a product entity.
    /// </summary>
    /// <param name="product">The product to map from.</param>
    /// <returns>A product dto.</returns>
    public static ProductDto FromProduct(Product product)
    {
        (decimal amountExcludingTax, decimal tax, decimal total) price = product.Price.CalculateComponents();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            AmountExcludingTax = price.amountExcludingTax,
            Tax = price.tax,
            Total = price.total,
        };
    }
}
