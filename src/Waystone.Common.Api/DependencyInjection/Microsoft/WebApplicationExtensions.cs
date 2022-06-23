// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;

/// <summary>Extensions for the <see cref="WebApplication" />.</summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Creates a new instance of <see cref="WaystoneApiApplicationBuilder" /> so that it can be used to configure the
    /// middleware
    /// provided by the library.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication" />.</param>
    /// <returns>Returns the <see cref="WaystoneApiApplicationBuilder" />.</returns>
    public static IWaystoneApiApplicationBuilder UseWaystoneApiApplicationBuilder(this WebApplication app)
    {
        return new WaystoneApiApplicationBuilder(app);
    }
}
