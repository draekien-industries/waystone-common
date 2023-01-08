namespace Waystone.Sample.Application.Products.ListProducts;

using Common.Application.Contracts.Pagination;

/// <summary>
/// List a page of products.
/// </summary>
public sealed class ListProductsQuery : PaginatedRequest<ProductDto>
{ }
