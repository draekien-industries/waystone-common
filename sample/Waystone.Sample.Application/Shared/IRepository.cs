namespace Waystone.Sample.Application.Shared;

using Domain.Products;
using Microsoft.EntityFrameworkCore;

public interface IRepository
{
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
