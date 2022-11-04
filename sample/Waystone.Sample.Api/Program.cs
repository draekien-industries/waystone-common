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

    builder.Services.AddWaystoneApiServiceBuilder(
                builder.Environment,
                builder.Configuration,
                typeof(ApplicationAssemblyMarker))
           .AcceptDefaults("Waystone.Sample.Api", "v1", "A sample API built with Waystone.Common.Api");

    builder.Services.AddSampleApplication();
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
    Log.Fatal(ex, "Host terminated unexpectedly. Check the WebHost configuration");

    throw;
}
finally
{
    Log.Information("Waystone.Sample.Api stopped");
    Log.CloseAndFlush();
}

/// <summary>Expose Program for integration tests</summary>
public partial class Program
{ }
