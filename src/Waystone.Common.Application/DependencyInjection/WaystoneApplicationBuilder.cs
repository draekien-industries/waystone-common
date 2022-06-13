namespace Waystone.Common.Application.DependencyInjection;

using System.Reflection;
using Contracts.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Builder for configuring dependencies in the application.</summary>
public class WaystoneApplicationBuilder : IWaystoneApplicationBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneApplicationBuilder" /> class.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblyMarkers"></param>
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
