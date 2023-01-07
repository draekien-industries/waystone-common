namespace Waystone.Sample.Domain.Products;

using Common.Domain.Contracts.Primitives;
using Common.Domain.Contracts.Results;
using Prices;

public class Product : Entity<Guid>
{
    public const int NameMaxLength = 255;
    public const int DescriptionMaxLength = 1280;

    private Product()
    { }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public Price Price { get; private set; }

    public Result<Product> UpdateName(string name)
    {
        Result validationResult = ProductValidators.ValidateName(name);

        if (validationResult.Failed)
        {
            return validationResult.Errors;
        }

        Name = name;

        return this;
    }

    public Result<Product> UpdateDescription(string description)
    {
        Result validationResult = ProductValidators.ValidateDescription(description);

        if (validationResult.Failed)
        {
            return validationResult.Errors;
        }

        Description = description;

        return this;
    }

    public Result<Product> UpdatePrice(Price price)
    {
        Result validationResult = ProductValidators.ValidatePrice(price);

        if (validationResult.Failed)
        {
            return validationResult.Errors;
        }

        Price = price;

        return this;
    }

    /// <summary>
    /// Creates a new product entity in a transient state
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <param name="price">The price of the product.</param>
    /// <param name="description">An optional product description.</param>
    /// <returns></returns>
    public static Result<Product> CreateTransient(string name, Price price, string description = "")
    {
        Result[] validationResults =
        {
            ProductValidators.ValidateName(name),
            ProductValidators.ValidateDescription(description),
            ProductValidators.ValidatePrice(price),
        };

        if (validationResults.Any(result => result.Failed))
        {
            return validationResults.SelectMany(result => result.Errors).ToArray();
        }

        return new Product
        {
            Name = name,
            Price = price,
            Description = description,
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetSignatureComponents()
    {
        yield return Name;
        yield return Description;
        yield return Price;
    }
}
