namespace Waystone.Common.Domain.Contracts;

/// <summary>
/// An object that is immutable and has no identity. Learn more:
/// https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
/// </summary>
public abstract class ValueObject
{
    /// <summary>Gets the hashcode of the current value object.</summary>
    /// <returns>The hashcode.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
              .Select(component => component is not null ? component.GetHashCode() : 0)
              .Aggregate((x, y) => x ^ y);
    }

    /// <summary>Checks to see if the provided object is equal to the current instance of the value object.</summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>True if the to be compared object equals the current value object.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != GetType()) return false;

        return obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>Gets the object's equality components.</summary>
    /// <remarks>Use `yield return` to return each component that needs to be compared.</remarks>
    /// <returns>The set of equality components.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>Checks to see if one value object is equal to another value object.</summary>
    /// <param name="left">The object on the left of the equals operator.</param>
    /// <param name="right">The object on the right of the equals operator.</param>
    /// <returns>True when the left value equals the right value.</returns>
    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null) return false;

        return left is null || left.Equals(right);
    }


    /// <summary>Checks to see if one value object is not equal to another value object.</summary>
    /// <param name="left">The object on the left of the not equals operator.</param>
    /// <param name="right">The object on the right of the not equals operator.</param>
    /// <returns>True when the left value does not equal to the right value.</returns>
    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
    {
        return !EqualOperator(left, right);
    }
}
