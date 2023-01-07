namespace Waystone.Sample.Infrastructure;

using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSampleInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddWaystoneInfrastructureBuilder(configuration)
                .AddInMemoryCaching() // or invoke AddRedisCaching when using redis
                .AcceptDefaults();

        services.AddDbContext<IRepository, SampleDbContext>(
            options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("Database"),
                    optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(DependencyInjection).Assembly.FullName);
                    });
            });

        return services;
    }
}
