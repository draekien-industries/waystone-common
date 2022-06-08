namespace Waystone.Common.Application.Contracts.DependencyInjection;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Application layer dependency injection registration interface.
/// </summary>
public interface IWaystoneApplicationBuilder
{
    /// <summary>
    ///
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    ///
    /// </summary>
    ICollection<Type> AssemblyMarkers { get; }

    IEnumerable<Assembly> GetAssemblies();
}
