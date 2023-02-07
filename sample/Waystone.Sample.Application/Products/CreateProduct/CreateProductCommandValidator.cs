namespace Waystone.Sample.Application.Products.CreateProduct;

using FluentValidation;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AmountExcludingTax).NotEmpty().GreaterThan(0);
        RuleFor(x => x.TaxPercentage).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.DiscountPercentage).GreaterThanOrEqualTo(0).When(x => x.DiscountPercentage is not null);
    }
}
