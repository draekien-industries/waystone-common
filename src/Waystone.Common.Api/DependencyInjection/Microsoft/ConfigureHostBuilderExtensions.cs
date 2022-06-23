// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;

/// <summary>
/// Extensions for the <see cref="ConfigureHostBuilder" />.
/// </summary>
public static class ConfigureHostBuilderExtensions
{
    /// <summary>
    /// Creates a new instance of the <see cref="IWaystoneApiHostBuilder" /> for the given <paramref name="host" />.
    /// </summary>
    /// <param name="host">The <see cref="ConfigureHostBuilder" /> that requires configuration.</param>
    /// <returns>The <see cref="IWaystoneApiHostBuilder" />.</returns>
    public static IWaystoneApiHostBuilder UseWaystoneApiHostBuilder(this ConfigureHostBuilder host)
    {
        return new WaystoneApiHostBuilder(host);
    }
}
