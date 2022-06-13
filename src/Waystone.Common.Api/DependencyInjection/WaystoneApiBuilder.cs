namespace Waystone.Common.Api.DependencyInjection;

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>Builder for configuring the Waystone Common API settings and dependencies.</summary>
public class WaystoneApiBuilder : IWaystoneApiBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneApiBuilder" /> class.</summary>
    /// <param name="services">The service collection.</param>
    /// <param name="environment">The host environment.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblies">The assemblies containing implementations of contracts provided by this library.</param>
    /// <exception cref="ArgumentNullException">One or more arguments are null.</exception>
    public WaystoneApiBuilder(
        IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IHostEnvironment Environment { get; }

    /// <inheritdoc />
    public IConfiguration Configuration { get; }

    /// <inheritdoc />
    public IEnumerable<Assembly> Assemblies { get; }
}
