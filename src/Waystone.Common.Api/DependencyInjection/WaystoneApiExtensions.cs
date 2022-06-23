// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using Waystone.Common.Api.Middleware;

/// <summary>Extensions for the <see cref="IWaystoneApiApplicationBuilder" />.</summary>
public static class WaystoneApiExtensions
{
    /// <summary>Accepts all the default middlewares for <see cref="IWaystoneApiApplicationBuilder" />.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="app">The <see cref="IWaystoneApiApplicationBuilder" />.</param>
    public static void AcceptDefaults(this IWaystoneApiApplicationBuilder app)
    {
        app.UseCorrelationIdHeaderMiddleware();
        app.UseHttpContextDtoMiddleware();
        app.WebApplication.UseSerilogRequestLogging();
        app.WebApplication.UseHttpsRedirection();
        app.WebApplication.UseProblemDetails();
        app.WebApplication.UseHttpsRedirection();
        app.WebApplication.UseAuthorization();
        app.WebApplication.MapControllers();
    }

    /// <summary>Adds the correlation id header middleware.</summary>
    /// <param name="app">The <see cref="IWaystoneApiApplicationBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiApplicationBuilder" />.</returns>
    public static IWaystoneApiApplicationBuilder UseCorrelationIdHeaderMiddleware(
        this IWaystoneApiApplicationBuilder app)
    {
        app.WebApplication.UseMiddleware<CorrelationIdHeaderMiddleware>();

        return app;
    }

    /// <summary>Adds the http context dto middleware.</summary>
    /// <param name="app">The <see cref="IWaystoneApiApplicationBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApiApplicationBuilder" />.</returns>
    public static IWaystoneApiApplicationBuilder UseHttpContextDtoMiddleware(this IWaystoneApiApplicationBuilder app)
    {
        app.WebApplication.UseMiddleware<HttpContextDtoMiddleware>();

        return app;
    }
}
