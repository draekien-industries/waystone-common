// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Models;

/// <summary>
/// Metadata about the API that is being configured.
/// </summary>
/// <param name="Name">The name of the API.</param>
/// <param name="Version">The version of the API.</param>
/// <param name="Description">An optional API description.</param>
public sealed record ApiMetadata(string Name, string Version, string Description = "");
