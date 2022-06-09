namespace Waystone.Common.Api.DependencyInjection;

using Microsoft.AspNetCore.Builder;

public interface IWaystoneApi
{
    WebApplication WebApplication { get; }
}
