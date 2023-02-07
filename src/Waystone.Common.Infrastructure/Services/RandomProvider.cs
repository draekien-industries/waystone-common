namespace Waystone.Common.Infrastructure.Services;

using Application.Contracts.Services;

/// <inheritdoc />
internal sealed class RandomProvider : IRandomProvider
{
    /// <inheritdoc />
    public int Next()
    {
        return Random.Shared.Next();
    }

    /// <inheritdoc />
    public int Next(int maxValue)
    {
        return Random.Shared.Next(maxValue);
    }

    /// <inheritdoc />
    public int Next(int minValue, int maxValue)
    {
        return Random.Shared.Next(minValue, maxValue);
    }

    /// <inheritdoc />
    public long NextInt64()
    {
        return Random.Shared.NextInt64();
    }

    /// <inheritdoc />
    public long NextInt64(long maxValue)
    {
        return Random.Shared.NextInt64(maxValue);
    }

    /// <inheritdoc />
    public long NextInt64(long minValue, long maxValue)
    {
        return Random.Shared.NextInt64(minValue, maxValue);
    }

    /// <inheritdoc />
    public float NextSingle()
    {
        return Random.Shared.NextSingle();
    }

    /// <inheritdoc />
    public double NextDouble()
    {
        return Random.Shared.NextDouble();
    }

    /// <inheritdoc />
    public void NextBytes(byte[] buffer)
    {
        Random.Shared.NextBytes(buffer);
    }

    /// <inheritdoc />
    public void NextBytes(Span<byte> buffer)
    {
        Random.Shared.NextBytes(buffer);
    }
}
