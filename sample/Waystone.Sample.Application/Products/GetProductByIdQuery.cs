namespace Waystone.Sample.Application.Products;

using Domain.Products;
using Microsoft.EntityFrameworkCore;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>
{ }

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
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
