namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;

/// <summary>The contract used to register the Waystone API middleware.</summary>
public interface IWaystoneApi
{
    /// <inheritdoc cref="WebApplication" />
    WebApplication WebApplication { get; }
}
