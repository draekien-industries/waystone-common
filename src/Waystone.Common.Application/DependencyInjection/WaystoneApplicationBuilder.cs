// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using Configuration;

/// <summary>Builder for configuring dependencies in the application.</summary>
internal class WaystoneApplicationBuilder : IWaystoneApplicationBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneApplicationBuilder" /> class.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration"></param>
    /// <param name="assemblyMarkers">The assemblies that make up the consuming app's application layer.</param>
    /// <exception cref="ArgumentNullException">The services collection does not exist.</exception>
    public WaystoneApplicationBuilder(
        IServiceCollection services,
        IConfiguration configuration,
        ICollection<Type> assemblyMarkers)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        AssemblyMarkers = assemblyMarkers ?? throw new ArgumentNullException(nameof(assemblyMarkers));
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IConfiguration Configuration { get; }

    /// <inheritdoc />
    public ICollection<Type> AssemblyMarkers { get; }

    /// <inheritdoc />
    public IEnumerable<Assembly> GetAssemblies()
    {
        return AssemblyMarkers.Select(marker => marker.Assembly);
    }
}
