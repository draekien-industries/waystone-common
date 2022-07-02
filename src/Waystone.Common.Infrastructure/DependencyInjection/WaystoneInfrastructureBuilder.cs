// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using Configuration;

/// <summary>Builder for configuring infrastructure dependencies.</summary>
internal class WaystoneInfrastructureBuilder : IWaystoneInfrastructureBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneInfrastructureBuilder" /> class.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app's configuration.</param>
    /// <exception cref="ArgumentNullException">The service collection does not exist.</exception>
    public WaystoneInfrastructureBuilder(IServiceCollection services, IConfiguration configuration)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IConfiguration Configuration { get; }
}
