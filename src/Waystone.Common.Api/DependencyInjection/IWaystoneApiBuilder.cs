namespace Waystone.Common.Api.DependencyInjection;

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>The contract used to register the services provided by Waystone.Common.Api.</summary>
public interface IWaystoneApiBuilder
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
