namespace Waystone.Sample.Infrastructure.Products;

using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(Product.NameMaxLength).IsUnicode(false);
        builder.HasIndex(p => p.Name, "UIX_Products_Name").IsUnique();

        builder.Property(p => p.Description).HasMaxLength(Product.DescriptionMaxLength).IsUnicode(false);

        builder.OwnsOne(p => p.Price)
               .Property(p => p.AmountExcludingTax)
               .HasColumnName("AmountExcludingTax")
               .HasPrecision(18);

        builder.OwnsOne(p => p.Price)
               .Property(p => p.TaxPercentage)
               .HasColumnName("TaxPercentage")
               .HasPrecision(18);

        builder.OwnsOne(p => p.Price)
               .Property(p => p.DiscountPercentage)
               .HasColumnName("DiscountPercentage")
               .HasPrecision(18);
    }
}
