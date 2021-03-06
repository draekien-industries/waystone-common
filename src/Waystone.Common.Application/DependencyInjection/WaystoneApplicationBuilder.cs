// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;

/// <summary>Builder for configuring dependencies in the application.</summary>
internal class WaystoneApplicationBuilder : IWaystoneApplicationBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneApplicationBuilder" /> class.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblyMarkers">The assemblies that make up the consuming app's application layer.</param>
    /// <exception cref="ArgumentNullException">The services collection does not exist.</exception>
    public WaystoneApplicationBuilder(IServiceCollection services, ICollection<Type> assemblyMarkers)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        AssemblyMarkers = assemblyMarkers ?? throw new ArgumentNullException(nameof(assemblyMarkers));
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public ICollection<Type> AssemblyMarkers { get; }

    /// <inheritdoc />
    public IEnumerable<Assembly> GetAssemblies()
    {
        return AssemblyMarkers.Select(marker => marker.Assembly);
    }
}
