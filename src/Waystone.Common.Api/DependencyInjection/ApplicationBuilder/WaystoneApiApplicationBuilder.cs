// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;

/// <inheritdoc />
internal class WaystoneApiApplicationBuilder : IWaystoneApiApplicationBuilder
{
    /// <summary>Initializes a new instance of the <see cref="WaystoneApiApplicationBuilder" /> class.</summary>
    /// <param name="app">The <see cref="WebApplication" />.</param>
    public WaystoneApiApplicationBuilder(WebApplication app)
    {
        WebApplication = app;
    }

    /// <inheritdoc />
    public WebApplication WebApplication { get; }
}
