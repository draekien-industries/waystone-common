namespace Waystone.Common.Application.Contracts.DependencyInjection;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Application layer dependency injection registration interface.</summary>
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
