# Waystone.Common.Infrastructure

The foundational code required to create .NET 6 Clean Architecture Infrastructure layer projects for Draekien
Industries.
It is intended to be used alongside:

- [Waystone.Common.Api](https://www.nuget.org/packages/Waystone.Common.Api)
- [Waystone.Common.Application](https://www.nuget.org/packages/Waystone.Common.Application)
-
    - [Waystone.Common.Domain.Contracts](https://www.nuget.org/packages/Waystone.Common.Domain.Contracts)

## Recommended Usage

The below sample code is the recommended usage for this library. If you wish to
not use some features provided by the library, then see the [Advanced Usage](#advanced-usage)
section and do not use `AcceptDefaults()`.

```csharp
// *.Infrastructure.DependencyInjection.cs
using Waystone.Infrastructure.DependencyInjection;

// Accept the default configuration inside an IServiceCollection extension method
// Currently configures:
// - date time provider
// - date time offset provider
// - random provider
services.AddWaystoneInfrastructureBuilder()
        .AcceptDefaults();
```

## Advanced Usage

```csharp
// *.Infrastructure.DependencyInjection.cs
using Waystone.Infrastructure.DependencyInjection;

// inside an IServiceCollection extension method
services.AddWaystoneInfrastructureBuilder()
        .AddDateTimeProviders()
        .AddRandomProvider();
```
