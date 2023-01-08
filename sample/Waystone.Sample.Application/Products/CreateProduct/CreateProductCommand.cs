namespace Waystone.Sample.Application.Products.CreateProduct;

using Common.Application.Contracts.Mediator;
using Domain.Products;

/// <summary>
/// Command for creating a new <see cref="Product" />
/// </summary>
/// <param name="Name">The name of the new product.</param>
/// <param name="Description">The description of the new product.</param>
/// <param name="AmountExcludingTax">The amount to charge for the product (excluding tax).</param>
/// <param name="TaxPercentage">The percentage of tax to be changed on this product.</param>
/// <param name="DiscountPercentage">An optional discount that will be applied on the product's price.</param>
public sealed record CreateProductCommand(
    string Name,
    string? Description,
    decimal AmountExcludingTax,
    decimal TaxPercentage,
    decimal? DiscountPercentage) : ICommand<ProductDto>;
