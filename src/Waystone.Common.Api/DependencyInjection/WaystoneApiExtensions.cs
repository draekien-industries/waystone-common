namespace Waystone.Common.Api.DependencyInjection;

using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Middleware;
using Serilog;

/// <summary>Extensions for the <see cref="IWaystoneApi" />.</summary>
public static class WaystoneApiExtensions
{
    /// <summary>Accepts all the default middlewares for <see cref="IWaystoneApi" />.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="app">The <see cref="IWaystoneApi" />.</param>
    public static void AcceptDefaults(this IWaystoneApi app)
    {
        app.UseCorrelationIdHeaderMiddleware();
        app.WebApplication.UseSerilogRequestLogging();
        app.WebApplication.UseHttpsRedirection();
        app.WebApplication.UseProblemDetails();
        app.WebApplication.UseHttpsRedirection();
        app.WebApplication.UseAuthorization();
        app.WebApplication.MapControllers();
    }

    /// <summary>Adds the correlation id header middleware.</summary>
    /// <param name="app">The <see cref="IWaystoneApi" />.</param>
    /// <returns>The <see cref="IWaystoneApi" />.</returns>
    public static IWaystoneApi UseCorrelationIdHeaderMiddleware(this IWaystoneApi app)
    {
        app.WebApplication.UseMiddleware<CorrelationIdHeaderMiddleware>();

        return app;
    }
}
