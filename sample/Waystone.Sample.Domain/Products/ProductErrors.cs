namespace Waystone.Sample.Domain.Products;

using Common.Domain.Contracts.Results;

public static class ProductErrors
{
    public static Error UnNamed => new("Products_UnNamed", "A product cannot be created without a name.");

    public static Error NameTooLong => new(
        "Products_NameTooLong",
        $"A product's name must be less than {Product.NameMaxLength} characters.");

    public static Error DescriptionTooLong => new(
        "Products_DescriptionTooLong",
        $"A product's description must be less than {Product.DescriptionMaxLength} characters.");

    public static Error MissingPrice => new("Products_MissingPrice", "A product cannot be created without a price.");
}
