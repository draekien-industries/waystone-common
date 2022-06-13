# Waystone.Common.Api

The foundational code required to create .NET 6 Clean Architecture API layer projects for Draekien Industries.

## Recommended Usage

The below sample code is the recommended usage for this library. If you wish to
configure your own problem details mapping (i.e. you have your own exceptions
that must be mapped to specific status codes), then see the [Advanced Usage](#advanced-usage)
section.

```csharp
// program.cs
using Waystone.Common.Api;

// after creating the web application builder
builder.Host.UseSerilog(
    (context, configuration) => 
        configuration.ReadFrom.Configuration(context.Configuration)
                     .Enrich.WithCorrelationIdHeader(builder.Configuration)
);

// Accept the default configuration provided by Waystone.Common.Api.
// Currently adds:
// - controllers
// - problem details
// - swagger (nswag)
// - newtonsoft configuration
// - correlation id header options
builder.Services
       .AddWaystoneApiBuilder(
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
app.UseWaystoneApi()
   .AcceptDefaults();
```

## Advanced Usage

```csharp
// program.cs
using Waystone.Common.Api;

// after creating the web application builder
builder.Host.UseSerilog(
    (context, configuration) => 
        configuration.ReadFrom.Configuration(context.Configuration)
                     .Enrich.WithCorrelationIdHeader(builder.Configuration)
);

// Manually configure Waystone.Common.Api
builder.Services
       .AddWaystoneApiBuilder(
           builder.Environment,
           builder.Configuration,
           typeof(Program))
       .AddControllers()
       .AddProblemDetails(options => { /* configure problem details */ })
       .AddSwaggerDocumentation(
           apiName: "ACME",
           apiVersion: "v1",
           apiDescription: "An api for ACME")
       .BindCorrelationIdHeaderOptions();
       
// After creating the web application with `builder.Build()`
app.UseWaystoneApi()
   .UseCorrelationIdHeaderMiddleware();
```
