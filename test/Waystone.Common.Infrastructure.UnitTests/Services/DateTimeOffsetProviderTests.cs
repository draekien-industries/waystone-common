namespace Waystone.Common.Infrastructure.UnitTests.Services;

using Application.Contracts.Services;
using Infrastructure.Services;

public sealed class DateTimeOffsetProviderTests
{
    private readonly IDateTimeOffsetProvider _sut;

    public DateTimeOffsetProviderTests()
    {
        _sut = new DateTimeOffsetProvider();
    }

    [Fact]
    public void WhenInvokingNow_ThenShouldNotThrow()
    {
        // Assert
        _sut.Invoking(x => x.Now).Should().NotThrow();
    }

    [Fact]
    public void WhenInvokingUtcNow_ThenShouldNotThrow()
    {
        // Assert
        _sut.Invoking(x => x.UtcNow).Should().NotThrow();
    }
}
