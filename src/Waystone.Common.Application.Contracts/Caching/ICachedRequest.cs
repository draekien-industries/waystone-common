namespace Waystone.Common.Application.Contracts.Caching;

using MediatR;

/// <summary>
/// A mediator request where the result of the request can be cached. The response is serialized to JSON
/// and stored in the distributed cache.
/// </summary>
/// <typeparam name="TResponse">The response type. Must have a parameterless constructor.</typeparam>
public interface ICachedRequest<out TResponse> : IRequest<TResponse> where TResponse : new()
{
    /// <summary>
    /// The key to use when storing the response in the cache.
    /// </summary>
    public string CacheKey { get; }

    /// <summary>
    /// The time to live for the response in the cache. Defaults to 5 minutes when not specified.
    /// </summary>
    public TimeSpan CacheDuration { get; }
}
