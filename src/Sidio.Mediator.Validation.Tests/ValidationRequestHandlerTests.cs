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