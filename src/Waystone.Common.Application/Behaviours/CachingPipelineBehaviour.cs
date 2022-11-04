namespace Waystone.Common.Application.Behaviours;

using Contracts.Caching;
using MediatR;

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

    /// <summary>
    /// Creates a new instance of the <see cref="CachingPipelineBehaviour{TRequest,TResponse}" /> class.
    /// </summary>
    /// <param name="cache">The <see cref="IDistributedCacheFacade" />.</param>
    /// <exception cref="ArgumentNullException">
    /// The <see cref="IDistributedCacheFacade" /> has not been registered in the DI
    /// container.
    /// </exception>
    public CachingPipelineBehaviour(IDistributedCacheFacade cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (request is not ICachedRequest<TResponse> cachedRequest) return await next();

        string key = cachedRequest.CacheKey;
        TimeSpan duration = cachedRequest.CacheDuration;

        if (duration == default
         || duration <= TimeSpan.Zero)
        {
            duration = TimeSpan.FromMinutes(5);
        }

        var cachedResponse = await _cache.GetObjectAsync<TResponse>(key, cancellationToken);

        if (cachedResponse is not null) return cachedResponse;

        TResponse response = await next();

        await _cache.PutObjectAsync(key, response, duration, cancellationToken);

        return response;
    }
}
