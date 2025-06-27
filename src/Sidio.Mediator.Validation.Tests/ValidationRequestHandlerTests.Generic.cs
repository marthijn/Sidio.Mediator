using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Validation.Tests;

public sealed class ValidationRequestHandlerTestsTyped
{
    [Fact]
    public async Task HandleAsync_RequestIsNotValid_ReturnsValidationResults()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatorRequestHandlers(typeof(IAssemblyMarker));
        services.AddMediatorValidation(typeof(IAssemblyMarker));

        var serviceProvider = services.BuildServiceProvider();
        var request = new TestRequest();
        var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TestRequest, string>>();

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
        services.AddMediatorRequestHandlers(typeof(IAssemblyMarker));
        services.AddMediatorValidation(typeof(IAssemblyMarker));

        var serviceProvider = services.BuildServiceProvider();
        var request = new TestRequest
        {
            Name = "Valid name",
        };
        var requestHandler = serviceProvider.GetRequiredService<IRequestHandler<TestRequest, string>>();

        // Act
        var result = await requestHandler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ValidationErrors.Should().BeEmpty();
    }

    public sealed class TestRequest : IRequest<string>
    {
        public string? Name { get; set; }
    }

    public sealed class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<Result<string>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result<string>.Success(request.Name ?? throw new InvalidOperationException()));
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