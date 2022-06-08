namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class WaystoneApiBuilder : IWaystoneApiBuilder
{
    public WaystoneApiBuilder(IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IHostEnvironment Environment { get; }

    /// <inheritdoc />
    public IConfiguration Configuration { get; }
}
