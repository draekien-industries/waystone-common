namespace Waystone.Sample.Infrastructure.Shared;

using Application.Shared;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

internal class SampleDbContext : DbContext, IRepository
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    { }

    public DbSet<Product> Products => Set<Product>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
