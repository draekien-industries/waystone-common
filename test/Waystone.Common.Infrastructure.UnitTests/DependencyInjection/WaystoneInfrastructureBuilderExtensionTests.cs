namespace Waystone.Common.Infrastructure.UnitTests.DependencyInjection;

using Application.Contracts.Caching;
using Application.Contracts.Services;
using Infrastructure.Caching;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class WaystoneInfrastructureBuilderExtensionTests
{
    private readonly IWaystoneInfrastructureBuilder _builder;

    public WaystoneInfrastructureBuilderExtensionTests()
    {
        _builder = Substitute.For<IWaystoneInfrastructureBuilder>();
    }

    [Fact]
    public void WhenInvokingAcceptDefaults_ThenAddDateTimeProvidersAndRandomProvider()
    {
        _builder.AcceptDefaults();

        _builder.Services.ReceivedWithAnyArgs().AddSingleton<IDateTimeProvider, DateTimeProvider>();
        _builder.Services.ReceivedWithAnyArgs().AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();
        _builder.Services.ReceivedWithAnyArgs().AddSingleton<IRandomProvider, RandomProvider>();
    }

    [Fact]
    public void WhenInvokingAddRedisCaching_ThenAddRedisCaching()
    {
        _builder.AddRedisCaching();

        _builder.Services.ReceivedWithAnyArgs().AddStackExchangeRedisCache(options => { });
        _builder.Services.ReceivedWithAnyArgs().TryAddSingleton<IDistributedCacheFacade, DistributedCacheFacade>();
    }
}
