namespace Waystone.Sample.Domain.Products;

using Common.Domain.Primitives;
using Common.Domain.Results;
using Prices;

/// <summary>
/// A product that can be sold.
/// </summary>
public class Product : Entity<Guid>
{
    /// <summary>
    /// The max length of the product's name.
    /// </summary>
    public const int NameMaxLength = 255;

    /// <summary>
    /// The max length of the product description.
    /// </summary>
    public const int DescriptionMaxLength = 1280;

    private Product()
    { }

    /// <summary>
    /// The product name.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// The product description.
    /// </summary>
    public string Description { get; private set; } = default!;

    /// <summary>
    /// THe product's price.
    /// </summary>
    public Price Price { get; private set; } = default!;

    /// <summary>
    /// Updates the name of the product.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates the description of the product.
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates the price of the product.
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
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
