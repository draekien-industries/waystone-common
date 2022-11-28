namespace Waystone.Common.Infrastructure.Caching;

using Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

internal class DistributedCacheFacade : IDistributedCacheFacade
{
    private readonly IDistributedCache _cache;

    public DistributedCacheFacade(IDistributedCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <inheritdoc />
    public Task InvalidateKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }

    /// <inheritdoc />
    public Task RefreshKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.RefreshAsync(key, cancellationToken);
    }

    /// <inheritdoc />
    public Task PutAsync(
        string key,
        string value,
        TimeSpan expiresIn,
        CancellationToken cancellationToken = default)
    {
        return _cache.SetStringAsync(key, value, GetOptions(expiresIn), cancellationToken);
    }

    /// <inheritdoc />
    public Task PutObjectAsync<T>(
        string key,
        T value,
        TimeSpan expiresIn,
        CancellationToken cancellationToken = default) where T : new()
    {
        string json = JsonConvert.SerializeObject(value, Formatting.None);

        return _cache.SetStringAsync(key, json, GetOptions(expiresIn), cancellationToken);
    }

    /// <inheritdoc />
    public async Task PutStreamAsync(
        string key,
        Stream value,
        TimeSpan expiresIn,
        CancellationToken cancellationToken = default)
    {
        using MemoryStream memoryStream = new();

        await value.CopyToAsync(memoryStream, cancellationToken);
        await _cache.SetAsync(key, memoryStream.ToArray(), GetOptions(expiresIn), cancellationToken);
    }

    /// <inheritdoc />
    public Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return _cache.GetStringAsync(key, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<T?> GetObjectAsync<T>(string key, CancellationToken cancellationToken = default) where T : new()
    {
        string? value = await GetAsync(key, cancellationToken);

        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        var result = JsonConvert.DeserializeObject<T>(value);

        return result;
    }

    /// <inheritdoc />
    public async Task<Stream?> GetStreamAsync(string key, CancellationToken cancellationToken = default)
    {
        byte[]? value = await _cache.GetAsync(key, cancellationToken);

        if (value is null)
        {
            return default;
        }

        Stream stream = new MemoryStream();

        await stream.WriteAsync(value, cancellationToken);

        return stream;
    }

    private static DistributedCacheEntryOptions GetOptions(TimeSpan expiresIn)
    {
        return new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiresIn };
    }
}
