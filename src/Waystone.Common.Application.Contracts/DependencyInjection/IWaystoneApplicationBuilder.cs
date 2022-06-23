// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;

/// <summary>A builder that manages the dependency injection of the services provided by the library.</summary>
public interface IWaystoneApplicationBuilder
{
    /// <summary>The service collection.</summary>
    IServiceCollection Services { get; }

    /// <summary>The assemblies to scan for mapping profiles, abstract validators, and mediator requests / handlers.</summary>
    ICollection<Type> AssemblyMarkers { get; }

    /// <summary>Gets the assemblies from the <see cref="AssemblyMarkers" />.</summary>
    /// <returns>The set of assemblies.</returns>
    IEnumerable<Assembly> GetAssemblies();
}
