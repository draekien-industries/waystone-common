namespace Waystone.Sample.Domain.Prices;

using System.Globalization;
using Common.Domain.Contracts.Primitives;
using Common.Domain.Contracts.Results;

/// <summary>
/// A price represents the cost of purchasing a product or service.
/// </summary>
public class Price : ValueObject
{
    private Price()
    { }

    /// <summary>
    /// The base price, which does not include any taxes or discounts.
    /// </summary>
    /// <example>20.45</example>
    public decimal AmountExcludingTax { get; private set; }

    /// <summary>
    /// The percentage of tax to apply to the base price.
    /// </summary>
    /// <example>0.1</example>
    public decimal TaxPercentage { get; private set; }

    /// <summary>
    /// The percentage of discount to apply to the base price.
    /// </summary>
    /// <example>0.1</example>
    public decimal DiscountPercentage { get; private set; }

    private bool HasTax => TaxPercentage > 0;

    private bool HasDiscount => DiscountPercentage > 0;

    /// <summary>
    /// Creates a new instance of <see cref="Price" />
    /// </summary>
    /// <param name="amountExcludingTax">The amount to charge excluding any taxes.</param>
    /// <param name="taxPercentage">
    /// The percentage of tax to apply to the base price. <c>0.1</c> is
    /// <c>10%</c>.
    /// </param>
    /// <param name="discountPercentage">
    /// The percentage discount to apply to the base price. <c>0.1</c> is
    /// <c>10%</c>.
    /// </param>
    /// <returns>A result that contains the created price, or any errors encountered.</returns>
    public static Result<Price> Create(
        decimal amountExcludingTax,
        decimal taxPercentage,
        decimal discountPercentage = default)
    {
        Result[] results =
        {
            PriceValidators.ValidateAmountExcludingTax(amountExcludingTax),
            PriceValidators.ValidateTaxPercentage(taxPercentage),
            PriceValidators.ValidateDiscountPercentage(discountPercentage),
        };

        if (results.Any(r => r.Failed))
        {
            return results.SelectMany(x => x.Errors).ToArray();
        }

        return new Price
        {
            AmountExcludingTax = amountExcludingTax,
            TaxPercentage = taxPercentage,
            DiscountPercentage = discountPercentage,
        };
    }

    /// <summary>
    /// Updates the amount excluding tax component of the price.
    /// </summary>
    /// <param name="amount">The new amount to charge.</param>
    /// <returns>The updated price.</returns>
    public Result<Price> UpdateAmountExcludingTax(decimal amount)
    {
        Result validationResult = PriceValidators.ValidateAmountExcludingTax(amount);

        if (validationResult.Failed)
        {
            return validationResult.Errors.ToArray();
        }

        AmountExcludingTax = amount;

        return this;
    }

    /// <summary>
    /// Updates the tax percentage.
    /// </summary>
    /// <param name="percentage">The new tax percentage.</param>
    /// <returns>The updated price.</returns>
    public Result<Price> UpdateTaxPercentage(decimal percentage)
    {
        Result validationResult = PriceValidators.ValidateTaxPercentage(percentage);

        if (validationResult.Failed)
        {
            return validationResult.Errors.ToArray();
        }

        TaxPercentage = percentage;

        return this;
    }

    /// <summary>
    /// Updates the discount percentage.
    /// </summary>
    /// <param name="percentage">The new discount percentage.</param>
    /// <returns>The updated price.</returns>
    public Result<Price> UpdateDiscountPercentage(decimal percentage)
    {
        Result validationResult = PriceValidators.ValidateDiscountPercentage(percentage);

        if (validationResult.Failed)
        {
            return validationResult.Errors.ToArray();
        }

        DiscountPercentage = percentage;

        return this;
    }

    /// <summary>
    /// Calculates the final price, taking into account any taxes and discounts.
    /// </summary>
    /// <returns>
    /// A tuple that contains: the amount (excluding tax) after any discounts, the calculated taxable component, and
    /// the total price.
    /// </returns>
    public (decimal amountExcludingTax, decimal tax, decimal total) CalculateComponents()
    {
        decimal tax, total;

        if (!HasDiscount)
        {
            tax = CalculateTax(AmountExcludingTax);
            total = AmountExcludingTax + tax;

            return (AmountExcludingTax, tax, total);
        }

        decimal discount = AmountExcludingTax * DiscountPercentage;
        decimal discountedAmountExcludingTax = AmountExcludingTax - discount;

        tax = CalculateTax(discountedAmountExcludingTax);
        total = discountedAmountExcludingTax + tax;

        return (discountedAmountExcludingTax, tax, total);
    }

    private decimal CalculateTax(decimal amountExcludingTax)
    {
        if (!HasTax) return amountExcludingTax;

        return amountExcludingTax * TaxPercentage;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AmountExcludingTax.ToString(CultureInfo.InvariantCulture);
        yield return TaxPercentage.ToString(CultureInfo.InvariantCulture);
        yield return DiscountPercentage.ToString(CultureInfo.InvariantCulture);
    }
}
