<p align="center">
  <img src="docs/logo.svg" alt="logo">
</p>

<h1 align="center">Waystone Common</h1>

- [Introduction](#introduction)
- [Getting Started](#getting-started)

## Introduction

Waystone Common contains the foundational code required to create Clean Architecture based .NET 6 applications for
Draekien-Industries. It is inspired by
this [Clean Architecture Template Repository](https://github.com/jasontaylordev/CleanArchitecture).

## Getting Started

In your project / solution, set up the following `.csproj` files, replacing `Acme` with your project's name:

- `Acme.Api.csproj`: The API layer of your app
- `Acme.Application.csproj`: The application layer of your app
- `Acme.Domain.csproj`: The domain layer of your app
- `Acme.Infrastructure.csproj`: The infrastructure layer of your app

Take a look at the `sample` directory of this repository for an example, or check
out [this great video](https://www.youtube.com/watch?v=dK4Yb6-LxAk).

Once your projects are set up, install the following NuGet packages according to each layer:

- `Acme.Api.csproj`: Install `Waystone.Common.Api` and follow
  the [usage instructions](src/Waystone.Common.Api/README.md)
- `Acme.Application.csproj`: Install `Waystone.Common.Application` and follow
  the [usage instructions](src/Waystone.Common.Application/README.md)
- `Acme.Infrastructure.csproj`: Install `Waystone.Common.Infrastructure` and follow
  the [usage instructions](src/Waystone.Common.Infrastructure/README.md)
