// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using Waystone.Common.Application.Contracts.Caching;

/// <summary>
/// Options for configuring the <see cref="IWaystoneApplicationBuilder" />.
/// </summary>
public class WaystoneApplicationBuilderOptions
{
    /// <summary>
    /// When set to `true`, registers a behaviour that automatically validates requests using Fluent Validation. Defaults to
    /// `true`.
    /// </summary>
    public bool UseValidationPipelineBehaviour { get; set; } = true;

    /// <summary>
    /// When set to `true`, registers a behaviour that caches requests that implement the
    /// <see cref="ICachedRequest{TResponse}" /> interface. Defaults to `true`.
    /// </summary>
    public bool UseCachingPipelineBehaviour { get; set; } = true;
}
