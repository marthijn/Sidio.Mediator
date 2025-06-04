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
        serviceCollection.AddMediator(typeof(MediatorTests));
        serviceCollection.AddScoped<IMediator, Mediator>();

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
        serviceCollection.AddMediator(typeof(MediatorTests));
        serviceCollection.AddScoped<IMediator, Mediator>();

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
        serviceCollection.AddMediator(typeof(MediatorTests));
        serviceCollection.AddScoped<IMediator, Mediator>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new TestRequest3();

        // Act
        var result = await mediator.TestRequest3Async(request);

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
        serviceCollection.AddMediator(typeof(MediatorTests)).AddMediatorValidation(typeof(MediatorTests));
        serviceCollection.AddScoped<IMediator, Mediator>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var request = new RequestWithValidation(input);

        // Act
        var result = await mediator.RequestWithValidationAsync(request);

        // Assert
        Assert.Equal(expectedResult, result.IsSuccess);
    }
}