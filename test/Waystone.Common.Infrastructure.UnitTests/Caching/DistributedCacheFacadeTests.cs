namespace Waystone.Common.Infrastructure.UnitTests.Caching;

using System.Text;
using Application.Contracts.Caching;
using Infrastructure.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute.ReturnsExtensions;

public class DistributedCacheFacadeTests
{
    private readonly IDistributedCache _distributedCache;
    private readonly TimeSpan _expiry;
    private readonly IFixture _fixture;
    private readonly DistributedCacheEntryOptions _options;
    private readonly IDistributedCacheFacade _sut;

    public DistributedCacheFacadeTests()
    {
        _fixture = new Fixture();
        _fixture.Inject(Stream.Null);
        _expiry = TimeSpan.FromMinutes(15);
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _expiry,
        };
        _distributedCache = Substitute.For<IDistributedCache>();
        _sut = new DistributedCacheFacade(_distributedCache, Options.Create(new DefaultCacheOptions()));
    }

    [Fact]
    public async Task GivenAKeyAndObject_WhenInvokingPutObjectAsync_ThenSerializeTheObjectAsJson_AndSetValueInCache()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var value = _fixture.Create<ObjectToStore>();
        string? serialized = JsonConvert.SerializeObject(value, Formatting.None);

        // Act
        await _sut.PutObjectAsync(key, value, _options);

        // Assert
        await _distributedCache.Received(1)
                               .SetAsync(
                                    key,
                                    Arg.Is<byte[]>(b => b.SequenceEqual(Encoding.UTF8.GetBytes(serialized))),
                                    Arg.Is<DistributedCacheEntryOptions>(
                                        d => d.AbsoluteExpirationRelativeToNow == _expiry));
    }

    [Fact]
    public async Task GivenAKeyAndAStream_WhenInvokingPutStreamAsync_ThenCopyTheStream_AndSetValueInCache()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var stream = _fixture.Create<Stream>();
        MemoryStream memoryStream = new();

        await stream.CopyToAsync(memoryStream);

        // Act
        await _sut.PutStreamAsync(key, stream, _options);

        // Assert
        await _distributedCache.Received(1)
                               .SetAsync(
                                    key,
                                    Arg.Is<byte[]>(b => b.SequenceEqual(memoryStream.ToArray())),
                                    Arg.Is<DistributedCacheEntryOptions>(
                                        d => d.AbsoluteExpirationRelativeToNow == _expiry));

        await memoryStream.DisposeAsync();
    }

    [Fact]
    public async Task GivenAKeyForACachedObject_WhenInvokingGetObjectAsync_ThenReturnTheExpectedObject()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var storedObject = _fixture.Create<ObjectToStore>();
        string? json = JsonConvert.SerializeObject(storedObject);

        _distributedCache.GetAsync(key).Returns(Encoding.UTF8.GetBytes(json));

        // Act
        var result = await _sut.GetObjectAsync<ObjectToStore>(key);

        // Assert
        result.Should().BeEquivalentTo(storedObject);
    }

    [Fact]
    public async Task GivenAKeyForAnObjectThatIsNotCached_WhenInvokingGetObjectAsync_ThenReturnNull()
    {
        // Arrange
        var key = _fixture.Create<string>();

        _distributedCache.GetAsync(key).ReturnsNull();

        // Act
        var result = await _sut.GetObjectAsync<ObjectToStore>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GivenStoredValue_WhenInvokingGetOrCreateAsync_ThenReturnStoredValue()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var value = _fixture.Create<string>();

        _distributedCache.GetAsync(key).Returns(Encoding.UTF8.GetBytes(value));

        // Act
        string result = await _sut.GetOrCreateAsync(key, () => Task.FromResult(value));

        // Assert
        await _distributedCache.Received(0).SetAsync(key, Encoding.UTF8.GetBytes(value));
        result.Should().Be(value);
    }

    [Fact]
    public async Task GivenNoStoredValue_WhenInvokingGetOrCreateAsync_ThenCreateValueFromFactory()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var value = _fixture.Create<string>();
        Func<Task<string>> factory = () => Task.FromResult(value);

        _distributedCache.GetAsync(key).ReturnsNull();

        // Act
        string result = await _sut.GetOrCreateAsync(key, factory);

        // Assert
        await _distributedCache.Received(1)
                               .SetAsync(
                                    key,
                                    Arg.Is<byte[]>(b => b.SequenceEqual(Encoding.UTF8.GetBytes(value))),
                                    Arg.Any<DistributedCacheEntryOptions>());
        result.Should().Be(value);
    }

    private class ObjectToStore
    {
        public int Id { get; set; }

        public string Value { get; set; } = string.Empty;
    }
}
