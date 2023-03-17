namespace Waystone.Sample.Application.Products.GetProduct;

using Common.Application.Contracts.Mediator;
using Common.Domain.Results;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IRepository _repository;

    public GetProductByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await _repository.Products.FirstOrDefaultAsync(
                product => product.Id.Equals(request.Id),
                cancellationToken);

            if (product is null)
            {
                return ProductsErrors.NotFound;
            }

            return ProductDto.FromProduct(product);
        }
        catch (Exception ex)
        {
            return new Error("Products_GetFailed", ex.Message, ex);
        }
    }
}
