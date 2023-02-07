// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using FluentValidation;
using MediatR;
using Waystone.Common.Application.Behaviours;
using Waystone.Common.Application.Contracts.Caching;
using Waystone.Common.Application.Mappings;

/// <summary>
/// Extensions for configuring the dependency injection provided by the Waystone Application.
/// </summary>
[PublicAPI]
public static class WaystoneApplicationBuilderExtensions
{
    /// <summary>
    /// Accept the default configuration of services for the Waystone Common Application. Adds the following services:
    /// - AutoMapper
    /// - MediatR
    /// - FluentValidation
    /// - FluentValidation pipeline behavior
    /// </summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    public static void AcceptDefaults(this IWaystoneApplicationBuilder builder)
    {
        builder.AddAutoMapper()
               .AddFluentValidation()
               .AddMediatR()
               .AddValidationPipelineBehaviour();
    }

    /// <summary>
    /// Configures the pipeline behaviours used by MediatR based on the options specified.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <param name="options">The action to configure the <see cref="WaystoneApplicationBuilderOptions" />.</param>
    public static void ConfigurePipelineBehaviours(
        this IWaystoneApplicationBuilder builder,
        Action<WaystoneApplicationBuilderOptions> options)
    {
        WaystoneApplicationBuilderOptions optionsInstance = new();

        options(optionsInstance);

        if (optionsInstance.UseValidationPipelineBehaviour)
        {
            builder.AddValidationPipelineBehaviour();
        }

        if (optionsInstance.UseCachingPipelineBehaviour)
        {
            builder.AddCachingPipelineBehaviour();
        }
    }

    /// <summary>
    /// Adds <see cref="AutoMapper" /> mappings to the dependency injection container, including all instances of
    /// <see cref="Waystone.Common.Application.Contracts.Mappings.IMapFrom{T}" /> in the provided assembly markers.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" />.</param>
    /// <returns>The <see cref="IWaystoneApplicationBuilder" />.</returns>
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
    /// <returns>The <see cref="IWaystoneApplicationBuilder" />.</returns>
    public static IWaystoneApplicationBuilder AddFluentValidation(
        this IWaystoneApplicationBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        builder.Services.AddValidatorsFromAssemblies(builder.GetAssemblies(), lifetime);

        return builder;
    }

    /// <summary>Adds all MediatR requests and handlers in the specified assemblies to the dependency injection container.</summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime" />.</param>
    /// <returns>The <see cref="IWaystoneApplicationBuilder" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The requested <see cref="ServiceLifetime" /> is not supported by MediatR.</exception>
    public static IWaystoneApplicationBuilder AddMediatR(
        this IWaystoneApplicationBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        builder.Services.AddMediatR(builder.GetAssemblies(), config => ConfigureMediatrLifetime(config, lifetime));

        return builder;
    }

    /// <summary>
    /// Registers a <see cref="IPipelineBehavior{TRequest,TResponse}" /> to add validation to all MediatR requests that
    /// have at least one associated <see cref="AbstractValidator{T}" />.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApplicationBuilder" />.</returns>
    public static IWaystoneApplicationBuilder AddValidationPipelineBehaviour(this IWaystoneApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

        return builder;
    }

    /// <summary>
    /// Registers a <see cref="IPipelineBehavior{TRequest,TResponse}" /> to add caching to all MediatR requests that
    /// implement <see cref="ICachedRequest{TResponse}" />.
    /// </summary>
    /// <param name="builder">The <see cref="IWaystoneApplicationBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneApplicationBuilder" />.</returns>
    public static IWaystoneApplicationBuilder AddCachingPipelineBehaviour(this IWaystoneApplicationBuilder builder)
    {
        builder.Services.Configure<DefaultCacheOptions>(builder.Configuration.GetSection(nameof(DefaultCacheOptions)));
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingPipelineBehaviour<,>));

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
