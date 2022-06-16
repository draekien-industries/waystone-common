# Waystone.Common.Application

The foundational code required to create .NET 6 Clean Architecture Application layer projects for Draekien Industries.
It is intended to be used alongside:

- [Waystone.Common.Api](https://www.nuget.org/packages/Waystone.Common.Api)
- [Waystone.Common.Domain.Contracts](https://www.nuget.org/packages/Waystone.Common.Domain.Contracts)
- [Waystone.Common.Infrastructure](https://www.nuget.org/packages/Waystone.Common.Infrastructure)

## Recommended Usage

The below sample code is the recommended usage for this library. If you wish to
not use some features provided by the library, then see the [Advanced Usage](#advanced-usage)
section and do not use `AcceptDefaults()`.

```csharp
// *.Application.DependencyInjection.cs
using Waystone.Application.DependencyInjection;

// Accept the default configuration inside an IServiceCollection extension method
// Currently configures:
// - automapper
// - mediatr
// - fluent validation
services.AddWaystoneApplicationBuilder(typeof(DependencyInjection))
        .AcceptDefaults();
```

## Advanced Usage

```csharp
// *.Application.DependencyInjection.cs
using Waystone.Application.DependencyInjection;

// inside an IServiceCollection extension method
services.AddWaystoneApplicationBuilder(typeof(DependencyInjection))
        .AddAutoMapper()
        .AddMediatR()
        .AddFluentValidation();
```
