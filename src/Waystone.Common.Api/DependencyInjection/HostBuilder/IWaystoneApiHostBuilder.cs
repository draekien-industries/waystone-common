// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;

/// <summary>
/// The builder provided by Waystone Common API for configuring the <see cref="ConfigureHostBuilder" />.
/// </summary>
public interface IWaystoneApiHostBuilder
{
    /// <summary>
    /// The <see cref="ConfigureHostBuilder" />.
    /// </summary>
    ConfigureHostBuilder Host { get; }
}
