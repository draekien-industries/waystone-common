namespace Waystone.Sample.Domain.Products;

using Common.Domain.Results;
using Prices;

internal static class ProductValidators
{
    public static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ProductErrors.UnNamed;
        }

        if (name.Length > Product.NameMaxLength)
        {
            return ProductErrors.NameTooLong;
        }

        return Result.Success();
    }

    public static Result ValidateDescription(string description)
    {
        if (description.Length > Product.DescriptionMaxLength)
        {
            return ProductErrors.DescriptionTooLong;
        }

        return Result.Success();
    }

    public static Result ValidatePrice(Price? price)
    {
        if (price is null)
        {
            return ProductErrors.MissingPrice;
        }

        return Result.Success();
    }
}
