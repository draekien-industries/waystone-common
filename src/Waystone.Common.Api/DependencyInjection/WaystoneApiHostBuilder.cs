// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;

internal class WaystoneApiHostBuilder : IWaystoneApiHostBuilder
{
    public WaystoneApiHostBuilder(ConfigureHostBuilder host)
    {
        Host = host;
    }

    /// <inheritdoc />
    public ConfigureHostBuilder Host { get; }
}
