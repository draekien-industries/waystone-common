namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;

public static class WebApplicationExtensions
{
    public static IWaystoneApi UseWaystoneApi(this WebApplication app)
    {
        return new WaystoneApi(app);
    }
}
