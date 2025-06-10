using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMediatorCqrs_WithoutAssemblyMarker_ThrowsArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var exception = Record.Exception(() => services.AddMediatorCqrs());

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void AddMediatorCqrs_WithoutNullAssemblyMarker_ThrowsArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var exception = Record.Exception(() => services.AddMediatorCqrs(null!));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void AddMediatorCqrs_WithAssemblyMarker_RequestHandlersRegistered()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMediatorCqrs(typeof(TestRequestHandler));

        // Assert
        services.Count.Should().BeGreaterThan(0);

        var serviceProvider = services.BuildServiceProvider();

        var requestHandler = serviceProvider.GetService<IRequestHandler<TestRequest>>();
        requestHandler.Should().NotBeNull();
    }

    public sealed class TestRequest : IRequest;

    public sealed class TestRequestHandler : IRequestHandler<TestRequest>
    {
        public Task<Result> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }
}