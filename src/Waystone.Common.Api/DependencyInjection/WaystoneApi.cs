namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;

public class WaystoneApi : IWaystoneApi
{
    public WaystoneApi(WebApplication app)
    {
        WebApplication = app;
    }

    /// <inheritdoc />
    public WebApplication WebApplication { get; }
}
