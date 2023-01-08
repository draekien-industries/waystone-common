namespace Waystone.Sample.Application.Products.GetProduct;

using Common.Application.Contracts.Mediator;

/// <summary>
/// Gets a product using it's ID.
/// </summary>
/// <param name="Id">The ID of the product to retrieve.</param>
public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
