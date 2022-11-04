// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using Configuration;

/// <summary>
/// Infrastructure layer dependency injection registration interface.
/// </summary>
public interface IWaystoneInfrastructureBuilder
{
    /// <summary>
    /// The service collection.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// The application configuration.
    /// </summary>
    IConfiguration Configuration { get; }
}
