﻿using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMediatorRequestHandlers_WithoutAssemblyMarker_ThrowsArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var exception = Record.Exception(() => services.AddMediatorRequestHandlers());

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void AddMediatorRequestHandlers_WithoutNullAssemblyMarker_ThrowsArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var exception = Record.Exception(() => services.AddMediatorRequestHandlers(null!));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void AddMediatorRequestHandlers_WithAssemblyMarker_RequestHandlersRegistered()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddMediatorRequestHandlers(typeof(TestRequestHandler));

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