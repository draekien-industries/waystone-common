namespace Waystone.Sample.Application.Shared;

using Domain.Products;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// The data repository for the application's entities.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// The products db set.
    /// </summary>
    DbSet<Product> Products { get; }

    /// <summary>
    /// The users db set.
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Saves changes to the data sets.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
