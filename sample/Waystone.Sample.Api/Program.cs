using Microsoft.Extensions.DependencyInjection.Models;
using Serilog;
using Serilog.Debugging;
using Waystone.Sample.Application;
using Waystone.Sample.Infrastructure;
using ApplicationAssemblyMarker = Waystone.Sample.Application.DependencyInjection;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

Log.Information("Starting Waystone.Sample.Api");

try
{
    SelfLog.Enable(Console.WriteLine);

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    ApiMetadata api = new("Waystone.Sample.Api", "v1", "A sample API built with Waystone.Common.Api");

    builder.Services.AddWaystoneApiServiceBuilder(
                builder.Environment,
                builder.Configuration,
                typeof(ApplicationAssemblyMarker))
           .AcceptDefaults(api);

    builder.Services.AddSampleApplication(builder.Configuration);
    builder.Services.AddSampleInfrastructure(builder.Configuration, builder.Environment);

    builder.Host.UseWaystoneApiHostBuilder()
           .AcceptDefaults();

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseOpenApi();
        app.UseSwaggerUi3();
        app.UseReDoc(options => options.Path = "/docs");
    }

    app.UseWaystoneApiApplicationBuilder()
       .AcceptDefaults();

    await app.RunAsync();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;

    if (type.Equals("HostAbortedException", StringComparison.Ordinal))
    {
        Log.Information(ex, "Host startup was aborted: {Message}", ex.Message);

        return 0;
    }

    Log.Fatal(ex, "Host terminated unexpectedly. Check the WebHost configuration");

    return 1;
}
finally
{
    Log.Information("Waystone.Sample.Api stopped");
    Log.CloseAndFlush();
}

return 0;

/// <summary>Expose Program for integration tests</summary>
public partial class Program
{ }
