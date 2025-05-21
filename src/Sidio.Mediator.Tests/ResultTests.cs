namespace Sidio.Mediator.Tests;

public sealed class ResultTests
{
    private readonly Fixture _fixture = new ();

    [Fact]
    public void Success_ReturnsSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Failure_WithValidationErrors_ReturnsFailureResultWithValidationErrors()
    {
        // Arrange
        var validationErrors = _fixture.CreateMany<ValidationError>().ToList();

        // Act
        var result = Result.Failure(validationErrors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Failure_WithErrorCodeAndMessage_ReturnsFailureResultWithCodeAndMessage()
    {
        // Arrange
        var errorCode = _fixture.Create<string>();
        var errorMessage = _fixture.Create<string>();

        // Act
        var result = Result.Failure(errorCode, errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
    }
}