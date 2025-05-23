using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Validation.Tests;

public sealed class ValidationRequestHandlerTests
{
    [Fact]
    public async Task HandleAsync_RequestIsNotValid_ReturnsValidationResults()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediator(typeof(IAssemblyMarker));
        services.AddMediatorValidation(typeof(IAssemblyMarker));

        var serviceProvider = services.BuildServiceProvider();
        var request = new TestRequest();
        var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TestRequest>>();

        // Act
        var result = await requestHandler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_RequestIsValid_ReturnsResults()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediator(typeof(IAssemblyMarker));
        services.AddMediatorValidation(typeof(IAssemblyMarker));

        var serviceProvider = services.BuildServiceProvider();
        var request = new TestRequest
        {
            Name = "Valid Name"
        };
        var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TestRequest>>();

        // Act
        var result = await requestHandler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WithoutValidation_ReturnsResults()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediator(typeof(IAssemblyMarker));
        services.AddMediatorValidation(typeof(IAssemblyMarker));

        var serviceProvider = services.BuildServiceProvider();
        var request = new TestRequest2();
        var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TestRequest2>>();

        // Act
        var result = await requestHandler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ValidationErrors.Should().BeEmpty();
    }

    public sealed class TestRequest : IRequest
    {
        public string? Name { get; init; }
    }

    public sealed class TestRequestHandler : IRequestHandler<TestRequest>
    {
        public Task<Result> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }

    public sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public sealed class TestRequest2 : IRequest
    {
        public string? Name { get; init; }
    }

    public sealed class TestRequestHandler2 : IRequestHandler<TestRequest2>
    {
        public Task<Result> HandleAsync(TestRequest2 request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }
}