namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Middleware;

public static class WaystoneApiExtensions
{
    public static void AcceptDefaults(this IWaystoneApi app)
    {
        app.UseCorrelationIdHeaderMiddleware();
    }

    public static IWaystoneApi UseCorrelationIdHeaderMiddleware(this IWaystoneApi app)
    {
        app.WebApplication.UseMiddleware<CorrelationIdHeaderMiddleware>();

        return app;
    }
}
