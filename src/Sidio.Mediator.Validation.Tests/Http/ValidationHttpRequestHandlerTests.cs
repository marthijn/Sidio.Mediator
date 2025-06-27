using System.Net;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Mediator.Http;

namespace Sidio.Mediator.Validation.Tests.Http;

public sealed class ValidationHttpRequestHandlerTests
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
        var requestHandler = serviceProvider.GetRequiredService<IHttpRequestHandler<TestRequest, string>>();

        // Act
        var result = await requestHandler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
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
            Name = "Valid name"
        };
        var requestHandler = serviceProvider.GetRequiredService<IHttpRequestHandler<TestRequest, string>>();

        // Act
        var result = await requestHandler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.ValidationErrors.Should().BeEmpty();
    }

    public sealed class TestRequest : IHttpRequest<string>
    {
        public string? Name { get; set; }
    }

    public sealed class TestRequestHandler : IHttpRequestHandler<TestRequest, string>
    {
        public Task<HttpResult<string>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HttpResult<string>.Success(request.Name ?? throw new InvalidOperationException()));
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