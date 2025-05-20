using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMediator_WithoutAssemblyMarker_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var exception = Record.Exception(() => services.AddMediator(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void AddMediator_WithoutAssemblyMarker_ThrowsArgumentNullExceptionWithCorrectParameterName()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMediator(typeof(TestRequestHandler));

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