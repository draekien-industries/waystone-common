namespace Waystone.Sample.Domain.Prices;

using Common.Domain.Results;

internal static class PriceValidators
{
    public static Result ValidateAmountExcludingTax(decimal amountExcludingTax)
    {
        return Validate(nameof(amountExcludingTax), amountExcludingTax, 0);
    }

    public static Result ValidateTaxPercentage(decimal taxPercentage)
    {
        return Validate(nameof(taxPercentage), taxPercentage, 0, inclusive: true);
    }

    public static Result ValidateDiscountPercentage(decimal discountPercentage)
    {
        return Validate(nameof(discountPercentage), discountPercentage, 0, 1, true);
    }

    private static Result Validate(
        string propertyName,
        decimal value,
        decimal minimum,
        decimal? maximum = null,
        bool inclusive = false)
    {
        if (IsInRange())
        {
            return Result.Success();
        }

        ArgumentOutOfRangeException exception = new(
            propertyName,
            $"The value provided to '{propertyName}' is out of range.");

        Error error = PriceErrors.OutOfRange(minimum, maximum, inclusive, exception);

        return Result.Fail(error);

        bool IsInRange()
        {
            if (maximum is null)
            {
                return inclusive ? value >= minimum : value > minimum;
            }

            return inclusive ? value >= minimum && value <= maximum : value > minimum && value < maximum;
        }
    }
}
