﻿using System.Net;
using Sidio.Mediator.Http;

namespace Sidio.Mediator.Tests.Http;

public sealed class HttpResultTests
{
    private readonly Fixture _fixture = new ();

    [Fact]
    public void Ok_ReturnsSuccessResult()
    {
        // Arrange
        var value = _fixture.Create<string>();

        // Act
        var result = HttpResult<string>.Ok(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
        result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Ok_WhenResponseIsNull_ThrowException()
    {
        // Act
        var action = () => HttpResult<string>.Ok(null!);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void NoContent_ReturnsResult()
    {
        // Act
        var result = HttpResult<string>.NoContent();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
        result.HttpStatusCode.Should().Be(HttpStatusCode.NoContent);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Unauthorized_ReturnsResult()
    {
        // Act
        var result = HttpResult<string>.Unauthorized();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.HttpStatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void BadRequest_WithValidationErrors_ReturnsResult()
    {
        // Arrange
        var validationErrors = _fixture.CreateMany<ValidationError>().ToList();

        // Act
        var result = HttpResult<string>.BadRequest(validationErrors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Theory]
    [InlineData("errorCode", "errorMessage")]
    [InlineData(null, null)]
    public void BadRequest_ReturnsResult(string? errorCode, string? errorMessage)
    {
        // Act
        var result = HttpResult<string>.BadRequest(errorCode, errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void Success_ReturnsSuccessResult()
    {
        // Arrange
        var value = _fixture.Create<string>();

        // Act
        var result = HttpResult<string>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
        result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Failure_WithValidationErrors_ReturnsFailureResultWithValidationErrors()
    {
        // Arrange
        var validationErrors = _fixture.CreateMany<ValidationError>().ToList();
        var httpStatusCode = _fixture.Create<HttpStatusCode>();

        // Act
        var result = HttpResult<string>.Failure(httpStatusCode, validationErrors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HttpStatusCode.Should().Be(httpStatusCode);
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
        var httpStatusCode = _fixture.Create<HttpStatusCode>();

        // Act
        var result = HttpResult<string>.Failure(httpStatusCode, errorCode, errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.HttpStatusCode.Should().Be(httpStatusCode);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().Be(errorCode);
        result.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public void StatusCode_WithValidationErrors_ReturnsFailureResultWithValidationErrors()
    {
        // Arrange
        var httpStatusCode = _fixture.Create<HttpStatusCode>();

        // Act
        var result = HttpResult<string>.StatusCode(httpStatusCode);

        // Assert
        result.HttpStatusCode.Should().Be(httpStatusCode);
        result.ValidationErrors.Should().BeEmpty();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
    }
}