namespace Waystone.Common.Infrastructure.Contracts.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>Infrastructure layer dependency injection registration interface.</summary>
public interface IWaystoneInfrastructureBuilder
{
    /// <summary>The service collection.</summary>
    IServiceCollection Services { get; }
}
