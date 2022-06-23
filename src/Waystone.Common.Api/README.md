# Waystone.Common.Api

The foundational code required to create .NET 6 Clean Architecture API layer projects for Draekien Industries.
It is intended to be used alongside:

- [Waystone.Common.Application](https://www.nuget.org/packages/Waystone.Common.Application)
- [Waystone.Common.Infrastructure](https://www.nuget.org/packages/Waystone.Common.Infrastructure)

## Recommended Usage

The below sample code is the recommended usage for this library. If you wish to
configure your own problem details mapping (i.e. you have your own exceptions
that must be mapped to specific status codes), then see the [Advanced Usage](#advanced-usage)
section and do not use `AcceptDefaults()`.

```csharp
// program.cs
using Waystone.Common.Api;

// after creating the web application builder
// configures serilog using appsettings and enrichers:
// - Correlation ID Header Enricher
// - Http Request Context Enricher
builder.Host
       .UseWaystoneApiHostBuilder()
       .AcceptDefaults();

// Accept the default configuration provided by Waystone.Common.Api.
// Currently adds:
// - controllers
// - route naming conventions
// - problem details
// - swagger (nswag)
// - newtonsoft configuration
// - correlation id header options
// - http request context action filter
builder.Services
       .AddWaystoneApiServiceBuilder(
           builder.Environment,
           builder.Configuration,
           typeof(Program))
       .AcceptDefault(
           apiName: "ACME",
           apiVersion: "v1",
           apiDescription: "An api for ACME");

// After creating the web application with `builder.Build()`
// Currently adds:
// - correlation id header middleware
// - https redirection
// - problem details
// - authorization
// - map controllers
// - serilog request logging
// - http request context logging middleware
app.UseWaystoneApiApplicationBuilder()
   .AcceptDefaults();
```

## Advanced Usage

```csharp
// program.cs
using Waystone.Common.Api;

// after creating the web application builder
builder.Host.UseSerilog(
    (context, provider, configuration) => 
        configuration.ReadFrom.Configuration(context.Configuration)
                     .Enrich.WithCorrelationIdHeader(builder.Configuration, provider)
                     .Enrich.WithHttpContext(provider);
);

// Manually configure Waystone.Common.Api
builder.Services
       .AddWaystoneApiServiceBuilder(
           builder.Environment,
           builder.Configuration,
           typeof(Program))
       .AddControllers()
       .AddRoutingConventions()
       .AddProblemDetails(options => { /* configure problem details */ })
       .AddSwaggerDocumentation(
           apiName: "ACME",
           apiVersion: "v1",
           apiDescription: "An api for ACME")
       .BindCorrelationIdHeaderOptions();
       
// After creating the web application with `builder.Build()`
app.UseWaystoneApiApplicationBuilder()
   .UseCorrelationIdHeaderMiddleware()
   .UseHttpContextDtoMiddleware();

app.UseHttpsRedirection();
app.UseProblemDetails();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSerilogRequestLogging();
```
