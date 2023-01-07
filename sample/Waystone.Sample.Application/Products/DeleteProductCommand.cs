namespace Waystone.Sample.Application.Products;

using Domain.Products;
using Microsoft.EntityFrameworkCore;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result>
{ }

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IRepository _repository;

    public DeleteProductCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Product? product =
                await _repository.Products.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (product is null)
            {
                return Result.Success();
            }

            _repository.Products.Remove(product);

            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return new Error("Products_DeleteFailed", ex.Message, ex);
        }
    }
}
