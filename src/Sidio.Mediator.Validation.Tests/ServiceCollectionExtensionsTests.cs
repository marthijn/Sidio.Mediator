using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Validation.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMediatorValidation_WithoutAssemblyMarker_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var exception = Record.Exception(() => services.AddMediatorValidation(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void AddMediatorValidation_WithAssemblyMarker_ValidatorsRegistered()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMediatorValidation(typeof(ServiceCollectionExtensionsTests));

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetService<IValidator<TestRequest>>();
        validator.Should().NotBeNull();

        var requestHandler = serviceProvider.GetService<IRequestHandler<TestRequest>>();
        requestHandler.Should().BeNull();
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
}