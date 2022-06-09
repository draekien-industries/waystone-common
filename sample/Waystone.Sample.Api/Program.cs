using Hellang.Middleware.ProblemDetails;
using Serilog;
using Serilog.Debugging;
using Waystone.Common.Api.DependencyInjection;
using Waystone.Common.Api.Logging;
using Waystone.Sample.Application;
using Waystone.Sample.Infrastructure;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

Log.Information("Starting Waystone.Sample.Api");

try
{
    SelfLog.Enable(Console.WriteLine);

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(
        (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
                                                 .Enrich.WithCorrelationIdHeader(builder.Configuration));

    builder.Services.AddWaystoneApiBuilder(builder.Environment, builder.Configuration)
           .AcceptDefaults("Waystone.Sample.Api", "v1", "A sample API built with Waystone.Common.Api");

    builder.Services.AddSampleApplication();
    builder.Services.AddSampleInfrastructure();

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseOpenApi();
        app.UseSwaggerUi3();
    }

    app.UseWaystoneApi()
       .AcceptDefaults();

    app.UseProblemDetails();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

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
