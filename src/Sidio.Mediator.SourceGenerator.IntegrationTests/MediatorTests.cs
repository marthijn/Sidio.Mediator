using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;
using Sidio.Mediator.Validation;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests;

public sealed class MediatorTests
{
    [Fact]
    public async Task DefaultRequest()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMediatorCqrs(typeof(MediatorTests));
        serviceCollection.AddMediatorService();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new TestRequest1("test");

        // Act
        var result = await mediator.TestRequest1Async(request);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task TypedRequest()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMediatorCqrs(typeof(MediatorTests));
        serviceCollection.AddMediatorService();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new TestRequest2("test");

        // Act
        var result = await mediator.TestRequest2Async(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Hello test", result.Value);
    }

    [Fact]
    public async Task TypedRequest_ResultInDifferentNamespace()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMediatorCqrs(typeof(MediatorTests));
        serviceCollection.AddMediatorService();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new TestRequest3();

        // Act
        var result = await mediator.TestRequest3Async(request);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DoubleTypedRequest()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMediatorCqrs(typeof(MediatorTests));
        serviceCollection.AddMediatorService();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new TestRequest4();

        // Act
        var result = await mediator.TestRequest4Async(request);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("test", true)]
    [InlineData("", false)]
    public async Task DefaultRequestWithValidation(string input, bool expectedResult)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediatorCqrs(typeof(MediatorTests)).AddMediatorValidation(typeof(MediatorTests));
        serviceCollection.AddMediatorService();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new RequestWithValidation(input);

        // Act
        var result = await mediator.RequestWithValidationAsync(request);

        // Assert
        Assert.Equal(expectedResult, result.IsSuccess);
    }

    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Singleton)]
    public void AddMediatorService_ServiceAdded(ServiceLifetime serviceLifetime)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddMediatorService(serviceLifetime);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var serviceDescriptor = serviceCollection.FirstOrDefault(
            d => d.ServiceType == typeof(IMediator));
        var mediator = serviceProvider.GetService<IMediator>();

        // Assert
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(serviceLifetime, serviceDescriptor.Lifetime);
        Assert.NotNull(mediator);
    }
}