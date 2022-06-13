namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;

/// <inheritdoc />
public class WaystoneApi : IWaystoneApi
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneApi" /> class.</summary>
    /// <param name="app">The <see cref="WebApplication" />.</param>
    public WaystoneApi(WebApplication app)
    {
        WebApplication = app;
    }

    /// <inheritdoc />
    public WebApplication WebApplication { get; }
}
