namespace Waystone.Common.Infrastructure.Caching;

using Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

internal class DistributedCacheFacade : IDistributedCacheFacade
{
    private readonly IDistributedCache _cache;
    private readonly DefaultCacheOptions _defaultCacheOptions;

    public DistributedCacheFacade(IDistributedCache cache, IOptions<DefaultCacheOptions> defaultCacheOptions)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _defaultCacheOptions = defaultCacheOptions.Value;
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
    public async Task<string> GetOrCreateAsync(
        string key,
        Func<Task<string>> factory,
        DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        string? storedValue = await GetAsync(key, cancellationToken);

        if (!string.IsNullOrWhiteSpace(storedValue)) return storedValue;

        string valueToStore = await factory();

        await PutAsync(key, valueToStore, options, cancellationToken);

        return valueToStore;
    }

    /// <inheritdoc />
    public async Task<T> GetOrCreateObjectAsync<T>(
        string key,
        Func<Task<T>> factory,
        DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default) where T : new()
    {
        var storedValue = await GetObjectAsync<T>(key, cancellationToken);

        if (storedValue is not null) return storedValue;

        T valueToStore = await factory();

        await PutObjectAsync(key, valueToStore, options, cancellationToken);

        return valueToStore;
    }

    /// <inheritdoc />
    public async Task<Stream> GetOrCreateStreamAsync(
        string key,
        Func<Task<Stream>> factory,
        DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        Stream? storedValue = await GetStreamAsync(key, cancellationToken);

        if (storedValue is not null) return storedValue;

        Stream valueToStore = await factory();

        await PutStreamAsync(key, valueToStore, options, cancellationToken);

        return valueToStore;
    }

    /// <inheritdoc />
    public Task PutAsync(
        string key,
        string value,
        DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return _cache.SetStringAsync(key, value, GetFinalOptions(options), cancellationToken);
    }

    /// <inheritdoc />
    public Task PutObjectAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default) where T : new()
    {
        string json = JsonConvert.SerializeObject(value, Formatting.None);

        return _cache.SetStringAsync(key, json, GetFinalOptions(options), cancellationToken);
    }

    /// <inheritdoc />
    public async Task PutStreamAsync(
        string key,
        Stream value,
        DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        using MemoryStream memoryStream = new();

        await value.CopyToAsync(memoryStream, cancellationToken);
        await _cache.SetAsync(key, memoryStream.ToArray(), GetFinalOptions(options), cancellationToken);
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

    private DistributedCacheEntryOptions GetFinalOptions(DistributedCacheEntryOptions? options = default)
    {
        if (options is not null) return options;

        return new DistributedCacheEntryOptions
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_defaultCacheOptions.ExpirySeconds) };
    }
}
