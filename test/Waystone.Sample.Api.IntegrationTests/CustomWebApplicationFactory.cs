namespace Waystone.Sample.Api.IntegrationTests;

using System.Data.Common;
using Infrastructure.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(
            services =>
            {
                ServiceDescriptor? dbContextDescriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SampleDbContext>));

                services.Remove(dbContextDescriptor);

                ServiceDescriptor? dbConnectionDescriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddDbContext<SampleDbContext>(
                    (container, options) => { options.UseInMemoryDatabase("WaystoneSample"); });
            });

        builder.UseEnvironment("Development");

        base.ConfigureWebHost(builder);
    }
}
