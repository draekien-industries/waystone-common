// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using Configuration;
using Hosting;

/// <summary>The contract used to register the services provided by Waystone.Common.Api.</summary>
public interface IWaystoneApiServiceBuilder
{
    /// <inheritdoc cref="IServiceCollection" />
    IServiceCollection Services { get; }

    /// <inheritdoc cref="IHostEnvironment" />
    IHostEnvironment Environment { get; }

    /// <inheritdoc cref="IConfiguration" />
    IConfiguration Configuration { get; }

    /// <summary>The assemblies that implement the services provided by Waystone.Common.Api.</summary>
    IEnumerable<Assembly> Assemblies { get; }
}
