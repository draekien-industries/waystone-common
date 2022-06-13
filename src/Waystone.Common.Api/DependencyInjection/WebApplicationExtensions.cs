namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;

/// <summary>Extensions for the <see cref="WebApplication" />.</summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Creates a new instance of <see cref="WaystoneApi" /> so that it can be used to configure the middleware
    /// provided by the library.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication" />.</param>
    /// <returns>Returns the <see cref="WaystoneApi" />.</returns>
    public static IWaystoneApi UseWaystoneApi(this WebApplication app)
    {
        return new WaystoneApi(app);
    }
}
