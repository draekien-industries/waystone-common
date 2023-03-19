namespace Waystone.Common.Domain.UnitTests.Results;

using Domain.Results;

public class ResultExtensionsTests
{
    [Fact]
    public void GivenAFailedResult_WhenInvokingBind_ThenReturnErrors()
    {
        // Arrange
        Result<string> failedResult = Result.Fail<string>("err", "string");


        // Act
        Result<string> bindingResult = failedResult.Bind<string, string>(value => $"I'm doing something with {value}");

        // Assert
        bindingResult.Succeeded.Should().BeFalse();
        bindingResult.Errors.Should().Contain(new Error("err", "string"));
    }

    [Fact]
    public void GivenSuccessResult_WhenInvokingBind_ThenReturnSuccess()
    {
        // Arrange
        Result<string> successResult = Result.Success("abc");

        // Act
        Result<string> bindingResult = successResult.Bind(value => Result.Success($"My {value}"));

        // Assert
        bindingResult.Succeeded.Should().BeTrue();
    }

    [Fact]
    public void GivenFailedBinding_WhenInvokingMatch_ThenInvokeOnFailure()
    {
        // Arrange
        Result<string> bindingResult = Result.Create("abc")
                                             .Bind(value => Result.Fail<string>("err", value));

        // Act
        string matchResult = bindingResult.Match(value => value, errors => string.Join(' ', errors));

        // Assert
        matchResult.Should().Be("err: abc");
    }

    [Fact]
    public void GivenSuccessBinding_WhenInvokingMatch_ThenInvokeOnSuccess()
    {
        // Arrange
        Result<string> bindingResult = Result.Create("abc")
                                             .Bind(Result.Success);

        // Act
        string matchResult = bindingResult.Match(value => value, errors => string.Join(' ', errors));

        // Assert
        matchResult.Should().Be("abc");
    }
}
