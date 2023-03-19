namespace Waystone.Common.Domain.UnitTests.Results;

using System.Net;
using Domain.Results;

public class ErrorTests
{
    [Fact]
    public void GivenError_WhenInvokingToString_ThenStringShouldBeCodeAndMessage()
    {
        // Arrange
        Error error = new("err", "abc");

        // Act
        var result = error.ToString();

        // Assert
        result.Should().Be("err: abc");
    }

    [Fact]
    public void GivenHttpError_WhenInvokingToString_ThenStringShouldBeCodeAndStatusCodeAndMessage()
    {
        // Arrange
        HttpError error = new(HttpStatusCode.BadRequest, "err", "abc");

        // Act
        var result = error.ToString();

        // Assert
        result.Should().Be("err(BadRequest): abc");
    }
}
