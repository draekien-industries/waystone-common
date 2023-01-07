namespace Waystone.Sample.Application.Products;

using Common.Application.Contracts.Pagination;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

public sealed class ListProductsQuery : PaginatedRequest<ProductDto>
{ }

public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, Result<PaginatedResponse<ProductDto>>>
{
    private readonly IRepository _repository;

    public ListProductsQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result<PaginatedResponse<ProductDto>>> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Product> products =
                await _repository.Products
                                 .OrderBy(product => product.Name)
                                 .Skip(request.Cursor)
                                 .Take(request.Limit)
                                 .ToListAsync(cancellationToken);

            int total = await _repository.Products.CountAsync(cancellationToken);

            PaginatedResponse<ProductDto> result = new()
            {
                Results = products.Select(ProductDto.FromProduct),
                Total = total,
            };

            return result;
        }
        catch (Exception ex)
        {
            return new Error("Products_ListFailed", ex.Message, ex);
        }
    }
}
