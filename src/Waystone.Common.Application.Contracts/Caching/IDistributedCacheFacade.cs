namespace Waystone.Common.Application.Contracts.Caching;

/// <summary>
/// A facade for using the Redis as a distributed cache.
/// </summary>
public interface IDistributedCacheFacade
{
    /// <summary>
    /// Invalidates a key in the cache, removing it's value.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>A completed task.</returns>
    Task InvalidateKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes a key in the cache, resetting it's expiry duration.
    /// </summary>
    /// <param name="key">The key to refresh.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>A completed task.</returns>
    Task RefreshKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or Updates a key in the cache, setting it's value.
    /// </summary>
    /// <param name="key">The key to create or update.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <param name="expiresIn">The time interval in which the cache remains valid.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The completed task.</returns>
    Task PutAsync(string key, string value, TimeSpan expiresIn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or Updates a key in the cache, setting it's value.
    /// </summary>
    /// <remarks>
    /// The value is serialized to JSON before being stored.
    /// </remarks>
    /// <param name="key">The key to create or update.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <param name="expiresIn">The time interval in which the cache remains valid.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <typeparam name="T">The object's type. Must have a parameterless constructor.</typeparam>
    /// <returns>The completed task.</returns>
    Task PutObjectAsync<T>(string key, T value, TimeSpan expiresIn, CancellationToken cancellationToken = default)
        where T : new();

    /// <summary>
    /// Creates or Updates a key in the cache, setting it's value.
    /// </summary>
    /// <remarks>
    /// The stream is converted to a byte array before being stored.
    /// </remarks>
    /// <param name="key">The key to create or update.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <param name="expiresIn">The time interval in which the cache remains valid.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The completed task.</returns>
    Task PutStreamAsync(string key, Stream value, TimeSpan expiresIn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a value from the cache using the specified key.
    /// </summary>
    /// <param name="key">The key used to retrieve the cached value.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The value if it exists.</returns>
    Task<string?> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a value from the cache using the specified key.
    /// </summary>
    /// <remarks>
    /// The value is deserialized from JSON to an object of type T before being returned.
    /// </remarks>
    /// <param name="key">The key used to retrieve the cached value.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <typeparam name="T">The object's type. Must have a parameterless constructor.</typeparam>
    /// <returns>The value if it exists.</returns>
    Task<T?> GetObjectAsync<T>(string key, CancellationToken cancellationToken = default) where T : new();

    /// <summary>
    /// Gets a value from the cache using the specified key.
    /// </summary>
    /// <remarks>
    /// The value is written to a stream from a byte array before being returned.
    /// </remarks>
    /// <param name="key">The key used to retrieve the cached value.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The value if it exists.</returns>
    Task<Stream?> GetStreamAsync(string key, CancellationToken cancellationToken = default);
}
