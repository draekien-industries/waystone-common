namespace Waystone.Sample.Infrastructure.Users;

using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).ValueGeneratedOnAdd();

        builder.HasIndex(user => user.Username, "UIX_Users_Username").IsUnique();
        builder.HasIndex(user => user.EmailAddress, "UIX_Users_EmailAddress").IsUnique();

        builder.Property(user => user.Username).HasMaxLength(50).IsUnicode(false).IsRequired();
        builder.Property(user => user.EmailAddress).HasMaxLength(1280).IsUnicode(false).IsRequired();

        builder.Property(user => user.EmailVerified).HasDefaultValue(false);
    }
}
