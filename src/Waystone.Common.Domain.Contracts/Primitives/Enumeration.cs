namespace Waystone.Common.Domain.Contracts.Primitives;

using System.Reflection;
using static System.Reflection.BindingFlags;

/// <summary>
/// An alternate enumeration class to get around the limitations of Enum types.
/// </summary>
[PublicAPI]
public abstract class Enumeration : IComparable
{
    private string? _displayName;

    /// <summary>
    /// Creates a new instance of <see cref="Enumeration" />
    /// </summary>
    /// <param name="value">The integer value of the enum member.</param>
    /// <param name="name">The name of the enum member.</param>
    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// The value of the enumeration instance.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// The name of the enumeration instance.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// An optional display name for this enumeration instance.
    /// </summary>
    public string DisplayName
    {
        get => _displayName ?? Name;
        protected set => _displayName = value;
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is not Enumeration other)
        {
            throw new ArgumentException("Obj is not the same type as this instance", nameof(obj));
        }

        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// Compares the left enumeration to the right enumeration to determine whether they are the same.
    /// </summary>
    /// <param name="left">The left enumeration instance to compare.</param>
    /// <param name="right">The right enumeration instance to compare.</param>
    /// <returns>True when the left instance equals the right instance.</returns>
    public static bool operator ==(Enumeration? left, Enumeration? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null)) return false;
        if (ReferenceEquals(right, null)) return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Compares the left enumeration to the right enumeration to determine whether they are different.
    /// </summary>
    /// <param name="left">The left enumeration instance to compare.</param>
    /// <param name="right">The right enumeration instance to compare.</param>
    /// <returns>True when the left instance does not equal the right instance.</returns>
    public static bool operator !=(Enumeration? left, Enumeration? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Compares the left enumeration to the right enumeration to determine if the value of the left instance
    /// is greater than the value of the right instance.
    /// </summary>
    /// <param name="left">The left enumeration instance to compare.</param>
    /// <param name="right">The right enumeration instance to compare.</param>
    /// <returns>True when the left value is greater than the right value.</returns>
    public static bool operator >(Enumeration? left, Enumeration? right)
    {
        if (left is null) return false;
        if (right is null) return false;

        return left.Value > right.Value;
    }

    /// <summary>
    /// Compares the left enumeration to the right enumeration to determine if the value of the left instance
    /// is less than the value of the right instance.
    /// </summary>
    /// <param name="left">The left enumeration instance to compare.</param>
    /// <param name="right">The right enumeration instance to compare.</param>
    /// <returns>True when the left value is less than the right value.</returns>
    public static bool operator <(Enumeration? left, Enumeration? right)
    {
        if (left is null) return false;
        if (right is null) return false;

        return left.Value < right.Value;
    }

    /// <summary>
    /// Compares the left enumeration to the right enumeration to determine if the value of the left instance
    /// is greater than or equal to the value of the right instance.
    /// </summary>
    /// <param name="left">The left enumeration instance to compare.</param>
    /// <param name="right">The right enumeration instance to compare.</param>
    /// <returns>True when the left value is greater than or equal to the right value.</returns>
    public static bool operator >=(Enumeration? left, Enumeration? right)
    {
        if (left is null) return false;
        if (right is null) return false;

        return left.Value >= right.Value;
    }

    /// <summary>
    /// Compares the left enumeration to the right enumeration to determine if the value of the left instance
    /// is less than or equal to the value of the right instance.
    /// </summary>
    /// <param name="left">The left enumeration instance to compare.</param>
    /// <param name="right">The right enumeration instance to compare.</param>
    /// <returns>True when the left value is less than or equal to the right value.</returns>
    public static bool operator <=(Enumeration? left, Enumeration? right)
    {
        if (left is null) return false;
        if (right is null) return false;

        return left.Value <= right.Value;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration other) return false;

        bool typeMatches = GetType() == obj.GetType();
        bool valueMatches = Value.Equals(other.Value);
        bool nameMatches = Name.Equals(other.Name);

        return typeMatches && valueMatches && nameMatches;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Gets all declared enumeration members in a class that implements <see cref="Enumeration" />.
    /// </summary>
    /// <typeparam name="T">A class that inherits from <see cref="Enumeration" /></typeparam>
    /// <returns>All public static instances of <see cref="Enumeration" /> in the specified type.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        FieldInfo[] fields = typeof(T).GetFields(Public | Static | DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    /// <summary>
    /// Tries to parse a value string into an enumeration of type T.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="enumeration">The enumeration that matches the provided value.</param>
    /// <typeparam name="T">A class that inherits from <see cref="Enumeration" /></typeparam>
    /// <returns>True when the value was successfully parsed.</returns>
    public static bool TryParse<T>(string value, out T? enumeration) where T : Enumeration
    {
        if (TryParse(item => item.Name == value, out enumeration))
        {
            return true;
        }

        if (!int.TryParse(value, out int parsedValue))
        {
            return false;
        }

        return TryParse(item => item.Value == parsedValue, out enumeration);
    }

    /// <summary>
    /// Parses an integer value into an enumeration of type T.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <typeparam name="T">A class that inherits from <see cref="Enumeration" /></typeparam>
    /// <returns>The matching enumeration.</returns>
    public static T FromValue<T>(int value) where T : Enumeration
    {
        T matchingItem = Parse<T, int>(value, nameof(value), item => item.Value == value);

        return matchingItem;
    }

    /// <summary>
    /// Parses an string value into an enumeration of type T.
    /// </summary>
    /// <param name="name">The value to parse.</param>
    /// <typeparam name="T">A class that inherits from <see cref="Enumeration" /></typeparam>
    /// <returns>The matching enumeration.</returns>
    public static T FromName<T>(string name) where T : Enumeration
    {
        T matchingItem = Parse<T, string>(name, nameof(name), item => item.Name.Equals(name));

        return matchingItem;
    }

    /// <summary>
    /// Sets the optional display name of an enumeration instance.
    /// </summary>
    /// <param name="displayName">The display name.</param>
    /// <returns>The enumeration instance.</returns>
    protected Enumeration WithDisplayName(string displayName)
    {
        DisplayName = displayName;

        return this;
    }

    private static bool TryParse<TEnumeration>(Func<TEnumeration, bool> predicate, out TEnumeration? enumeration)
        where TEnumeration : Enumeration
    {
        enumeration = GetAll<TEnumeration>().FirstOrDefault(predicate);

        return enumeration is not null;
    }

    private static TEnumeration Parse<TEnumeration, TValue>(
        TValue value,
        string description,
        Func<TEnumeration, bool> predicate)
        where TEnumeration : Enumeration
    {
        TEnumeration? matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);

        if (matchingItem is null)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"'{value}' is not a valid {description} in {typeof(TEnumeration)}"
            );
        }

        return matchingItem;
    }
}
