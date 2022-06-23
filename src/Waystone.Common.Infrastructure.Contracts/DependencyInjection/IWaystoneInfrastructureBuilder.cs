// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>Infrastructure layer dependency injection registration interface.</summary>
public interface IWaystoneInfrastructureBuilder
{
    /// <summary>The service collection.</summary>
    IServiceCollection Services { get; }
}
