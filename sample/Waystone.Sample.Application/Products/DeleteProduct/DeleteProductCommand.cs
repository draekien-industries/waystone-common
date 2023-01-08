namespace Waystone.Sample.Application.Products.DeleteProduct;

using Common.Application.Contracts.Mediator;

/// <summary>
/// Deletes the product associated with the provided ID.
/// </summary>
/// <param name="Id">The ID of the product to delete.</param>
public sealed record DeleteProductCommand(Guid Id) : ICommand;
