// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using Caching.StackExchangeRedis;
using Configuration;
using StackExchange.Redis;
using Waystone.Common.Application.Contracts.Caching;
using Waystone.Common.Application.Contracts.Services;
using Waystone.Common.Domain.Contracts.Exceptions;
using Waystone.Common.Infrastructure.Caching;
using Waystone.Common.Infrastructure.Services;

/// <summary>Extensions for configuring the Waystone Common infrastructure dependency injection.</summary>
public static class WaystoneInfrastructureBuilderExtensions
{
    private const string RedisConnectionStringKey = "Redis";

    /// <summary>Accept the default configuration of services for the Waystone Common Infrastructure.</summary>
    /// <remarks>
    /// This is the recommended way of using this library. If you choose to use this method, you will not need to call
    /// any of the other methods in this class.
    /// </remarks>
    /// <param name="builder">The <see cref="IWaystoneInfrastructureBuilder" />.</param>
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

    /// <summary>
    /// Registers dependencies required to use redis for distributed caching. Provides a service facade
    /// for easier use of the redis cache. See <see cref="IDistributedCacheFacade" />.
    /// </summary>
    /// <remarks>
    /// Will attempt to use the connection string "Redis" if it exists. If not, will attempt to bind the configuration
    /// section "RedisConfigurationOptions". If neither of these exists, an <see cref="InvalidOperationException" />
    /// will be thrown.
    /// </remarks>
    /// <param name="builder">The <see cref="IWaystoneInfrastructureBuilder" />.</param>
    /// <returns>The <see cref="IWaystoneInfrastructureBuilder" />.</returns>
    public static IWaystoneInfrastructureBuilder AddRedisCaching(this IWaystoneInfrastructureBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options => ConfigureRedisCache(options, builder.Configuration));
        builder.Services.TryAddSingleton<IDistributedCacheFacade, DistributedCacheFacade>();

        return builder;
    }

    /// <summary>
    /// Configures the redis connection, prioritising a connection string, but falling back to a configuration section.
    /// </summary>
    /// <remarks>
    /// Will attempt to use the connection string "Redis" if it exists. If not, will attempt to bind the configuration
    /// section "RedisConfigurationOptions". If neither of these exists, an <see cref="InvalidOperationException" />
    /// will be thrown.
    /// </remarks>
    /// <param name="options">The <see cref="RedisCacheOptions" /> to configure.</param>
    /// <param name="configuration">The app's configuration.</param>
    /// <exception cref="InvalidOperationException">Failed to configure redis cache options.</exception>
    private static void ConfigureRedisCache(RedisCacheOptions options, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(RedisConnectionStringKey);

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            options.ConfigurationOptions = ConfigurationOptions.Parse(connectionString, true);

            return;
        }

        var redisConfig = configuration.GetRequiredSection(nameof(RedisCacheConfiguration))
                                       .Get<RedisCacheConfiguration>();

        if (!redisConfig.Endpoints.Any())
        {
            throw new InvalidConfigurationException("RedisCacheConfiguration:Endpoints");
        }

        ConfigurationOptions configurationOptions = new();

        foreach (string endpoint in redisConfig.Endpoints)
        {
            configurationOptions.EndPoints.Add(endpoint);
        }

        configurationOptions.AllowAdmin = redisConfig.AllowAdmin;
        configurationOptions.User = redisConfig.User;
        configurationOptions.Password = redisConfig.Password;
        configurationOptions.ClientName = redisConfig.ClientName ?? Assembly.GetExecutingAssembly().GetName().FullName;
        configurationOptions.DefaultDatabase = redisConfig.DefaultDatabase;
        configurationOptions.ServiceName = redisConfig.ServiceName;

        options.ConfigurationOptions = configurationOptions;
    }
}
