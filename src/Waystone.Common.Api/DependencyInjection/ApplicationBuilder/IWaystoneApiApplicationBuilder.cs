// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AspNetCore.Builder;

/// <summary>The contract used to register the Waystone API middleware.</summary>
public interface IWaystoneApiApplicationBuilder
{
    /// <inheritdoc cref="WebApplication" />
    WebApplication WebApplication { get; }
}
