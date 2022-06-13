namespace Waystone.Common.Application.Contracts.Services;

/// <summary>
/// Provides access to the static <see cref="Random" />.<see cref="Random.Shared" /> instance. Use this contract
/// in your app when you need to be able to mock the results of the random generator.
/// </summary>
public interface IRandomProvider
{
    /// <summary>Returns a non-negative random integer.</summary>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="int.MaxValue" />.</returns>
    int Next();

    /// <summary>Returns a non-negative random integer that is less than the specified maximum.</summary>
    /// <param name="maxValue">
    /// The exclusive upper bound of the random number to be generated. `maxValue` must be greater than
    /// or equal to 0.
    /// </param>
    /// <returns>
    /// A 32-bit signed integer that is greater than or equal to 0, and less than `maxValue`; that is, the range of
    /// return values ordinarily includes 0 but not `maxValue`. However if `maxValue` is 0, `maxValue` is returned.
    /// </returns>
    int Next(int maxValue);

    /// <summary>Returns a random integer that is within a specified range.</summary>
    /// <param name="minValue">The *inclusive* lower bound of the random number to be generated.</param>
    /// <param name="maxValue">
    /// The *exclusive* upper bound of the random number to be generated. `maxValue` must be greater
    /// than or equal to `minValue`.
    /// </param>
    /// <returns></returns>
    int Next(int minValue, int maxValue);

    /// <summary>Returns a non-negative random integer.</summary>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than <see cref="long.MaxValue" />.</returns>
    long NextInt64();

    /// <summary>Returns a non-negative random integer that is less than the specified maximum.</summary>
    /// <param name="maxValue">
    /// The exclusive upper bound of the random number to be generated. `maxValue` must be greater than
    /// or equal to 0.
    /// </param>
    /// <returns>
    /// A 64-bit signed integer that is greater than or equal to 0, and less than `maxValue`; that is, the range of
    /// return values ordinarily includes 0 but not `maxValue`. However if `maxValue` is 0, `maxValue` is returned.
    /// </returns>
    long NextInt64(long maxValue);

    /// <summary>Returns a random integer that is within a specified range.</summary>
    /// <param name="minValue">The *inclusive* lower bound of the random number to be generated.</param>
    /// <param name="maxValue">
    /// The *exclusive* upper bound of the random number to be generated. `maxValue` must be greater
    /// than or equal to `minValue`.
    /// </param>
    /// <returns>
    /// A 64-bit signed integer greater than or equal to `minValue` and less than `maxValue`; that is, the range of
    /// return values ordinarily includes 0 but not `maxValue`. However if `maxValue` is 0, `maxValue` is returned.
    /// </returns>
    long NextInt64(long minValue, long maxValue);

    /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
    /// <returns>A single-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    float NextSingle();

    /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
    /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    double NextDouble();

    /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
    /// <param name="buffer">The array to be filled with random numbers.</param>
    void NextBytes(byte[] buffer);

    /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
    /// <param name="buffer">The array to be filled with random numbers.</param>
    void NextBytes(Span<byte> buffer);
}
