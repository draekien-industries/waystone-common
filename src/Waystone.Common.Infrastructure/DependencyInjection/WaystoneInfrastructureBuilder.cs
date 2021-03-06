// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>Builder for configuring infrastructure dependencies.</summary>
internal class WaystoneInfrastructureBuilder : IWaystoneInfrastructureBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneInfrastructureBuilder" /> class.</summary>
    /// <param name="services">The service collection.</param>
    /// <exception cref="ArgumentNullException">The service collection does not exist.</exception>
    public WaystoneInfrastructureBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }
}
