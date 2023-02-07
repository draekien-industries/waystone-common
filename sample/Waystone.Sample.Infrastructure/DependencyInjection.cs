namespace Waystone.Sample.Infrastructure;

using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

/// <summary>
/// DI for sample infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures services used inside the infrastructure layer.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    public static IServiceCollection AddSampleInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddWaystoneInfrastructureBuilder(configuration)
                .AddInMemoryCaching() // or invoke AddRedisCaching when using redis
                .AcceptDefaults();

        services.AddDbContext<IRepository, SampleDbContext>(
            options =>
            {
                if (environment.IsDevelopment())
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }

                options.UseSqlServer(
                    configuration.GetConnectionString("Database"),
                    optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(DependencyInjection).Assembly.FullName);

                        optionsBuilder.EnableRetryOnFailure();
                    });
            });

        return services;
    }
}
