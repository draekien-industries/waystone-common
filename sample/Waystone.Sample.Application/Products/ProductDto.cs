namespace Waystone.Sample.Application.Products;

using Domain.Products;

public sealed class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal AmountExcludingTax { get; set; }

    public decimal Tax { get; set; }

    public decimal Total { get; set; }

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
