namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public interface IWaystoneApiBuilder
{
    IServiceCollection Services { get; }

    IHostEnvironment Environment { get; }

    IConfiguration Configuration { get; }
}
