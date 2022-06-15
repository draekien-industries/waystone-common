namespace Waystone.Common.Application.DependencyInjection;

using System.Reflection;
using Contracts.DependencyInjection;
using FluentValidation;
using Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>Extensions for configuring the dependency injection provided by the Waystone Application.</summary>
public static class WaystoneApplicationBuilderExtensions
{
    /// <summary>Accept the default configuration of services for the Waystone Common Application.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    public static void AcceptDefaults(this IWaystoneApplicationBuilder builder)
    {
        builder.AddAutoMapper()
               .AddMediatR()
               .AddFluentValidation();
    }

    /// <summary>
    /// Adds <see cref="AutoMapper" /> mappings to the dependency injection container, including all instances of
    /// <see cref="Waystone.Common.Application.Contracts.Mappings.IMapFrom{T}" /> in the provided assembly markers.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" />.</param>
    /// <returns></returns>
    public static IWaystoneApplicationBuilder AddAutoMapper(
        this IWaystoneApplicationBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        List<Assembly> assemblies = builder.GetAssemblies().ToList();

        assemblies.Add(Assembly.GetExecutingAssembly());

        builder.Services.AddAutoMapper(assemblies, lifetime);

        MappingProfile.AssemblyMarkersContainingIMapFrom = builder.AssemblyMarkers;

        return builder;
    }

    /// <summary>
    /// Adds all instances of fluent validation validators in the specified assemblies to the dependency injection
    /// container.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" />.</param>
    /// <returns></returns>
    public static IWaystoneApplicationBuilder AddFluentValidation(
        this IWaystoneApplicationBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        builder.Services.AddValidatorsFromAssemblies(builder.GetAssemblies(), lifetime);

        return builder;
    }

    /// <summary>Adds all MediatR requests and handlers in the specified assemblies to the dependency injection container.</summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" />.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">The requested <see cref="ServiceLifetime" /> is not supported by MediatR.</exception>
    public static IWaystoneApplicationBuilder AddMediatR(
        this IWaystoneApplicationBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        builder.Services.AddMediatR(builder.GetAssemblies(), config => ConfigureMediatrLifetime(config, lifetime));

        return builder;
    }

    private static void ConfigureMediatrLifetime(MediatRServiceConfiguration configuration, ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Scoped:
                configuration.AsScoped();

                break;
            case ServiceLifetime.Singleton:
                configuration.AsSingleton();

                break;
            case ServiceLifetime.Transient:
                configuration.AsTransient();

                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(lifetime),
                    lifetime,
                    "The provided lifetime is not supported.");
        }
    }
}
