// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using Waystone.Common.Application.Contracts.Services;
using Waystone.Common.Infrastructure.Services;

/// <summary>Extensions for configuring the Waystone Common infrastructure dependency injection.</summary>
public static class WaystoneInfrastructureBuilderExtensions
{
    /// <summary>Accept the default configuration of services for the Waystone Common Infrastructure.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="builder"></param>
    public static void AcceptDefaults(this IWaystoneInfrastructureBuilder builder)
    {
        builder.AddDateTimeProviders()
               .AddRandomProvider();
    }

    /// <summary>
    /// Adds the <see cref="IDateTimeProvider" /> and <see cref="IDateTimeOffsetProvider" /> to the
    /// <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneInfrastructureBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneInfrastructureBuilder" />.</returns>
    public static IWaystoneInfrastructureBuilder AddDateTimeProviders(this IWaystoneInfrastructureBuilder builder)
    {
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

        return builder;
    }

    /// <summary>Adds the <see cref="IRandomProvider" /> to the <see cref="IServiceCollection" />.</summary>
    /// <param name="builder">The <see cref="IWaystoneInfrastructureBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneInfrastructureBuilder" />.</returns>
    public static IWaystoneInfrastructureBuilder AddRandomProvider(this IWaystoneInfrastructureBuilder builder)
    {
        builder.Services.AddSingleton<IRandomProvider, RandomProvider>();

        return builder;
    }
}
