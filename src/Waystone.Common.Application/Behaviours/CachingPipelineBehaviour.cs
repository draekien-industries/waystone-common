namespace Waystone.Common.Application.Behaviours;

using Contracts.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

/// <summary>
/// A <see cref="IPipelineBehavior{TRequest,TResponse}" /> that caches the result of a request, storing
/// it as JSON in the distributed cache.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type. Must have a parameterless constructor.</typeparam>
internal sealed class CachingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : new()
{
    private readonly IDistributedCacheFacade _cache;
    private readonly DefaultCacheOptions _defaultCacheOptions;

    /// <summary>
    /// Creates a new instance of the <see cref="CachingPipelineBehaviour{TRequest,TResponse}" /> class.
    /// </summary>
    /// <param name="cache">The <see cref="IDistributedCacheFacade" />.</param>
    /// <param name="cacheOptions"></param>
    /// <exception cref="ArgumentNullException">
    /// The <see cref="IDistributedCacheFacade" /> has not been registered in the DI
    /// container.
    /// </exception>
    public CachingPipelineBehaviour(IDistributedCacheFacade cache, IOptions<DefaultCacheOptions> cacheOptions)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _defaultCacheOptions = cacheOptions.Value;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (request is not ICachedRequest<TResponse> cachedRequest) return await next();

        string key = cachedRequest.CacheKey;
        TimeSpan? duration = cachedRequest.CacheSeconds;

        if (duration <= TimeSpan.Zero)
        {
            duration = TimeSpan.FromSeconds(_defaultCacheOptions.ExpirySeconds);
        }

        return await _cache.GetOrCreateObjectAsync(
            key,
            () => next(),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration,
            },
            cancellationToken);
    }
}
