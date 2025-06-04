using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Sidio.Mediator.SourceGenerator.Tests;

public class SourceGeneratorTests
{
    [Fact]
    public Task Driver_DefaultRequest()
    {
        // Arrange
        var driver = BuildDriver(SourceCodeDefaultRequest);

        // Act
        _ = driver.GetRunResult().Results.Single();

        // Assert
        return Verify(driver);
    }

    [Fact]
    public Task Driver_TypedRequest()
    {
        // Arrange
        var driver = BuildDriver(SourceCodeTypedRequest);

        // Act
        _ = driver.GetRunResult().Results.Single();

        // Assert
        return Verify(driver);
    }

    [Fact]
    public Task Driver_HttpRequest()
    {
        // Arrange
        var driver = BuildDriver(SourceCodeHttpRequest);

        // Act
        _ = driver.GetRunResult().Results.Single();

        // Assert
        return Verify(driver);
    }

    private static GeneratorDriver BuildDriver(string sourceCode)
    {
        var compilation = CSharpCompilation.Create(typeof(SourceGeneratorTests).Assembly.FullName,
            new[] {CSharpSyntaxTree.ParseText(sourceCode)}
            );
        var generator = new MediatorGenerator();

        var driver = CSharpGeneratorDriver.Create(generator);
        return driver.RunGenerators(compilation);
    }


    private const string SourceCodeDefaultRequest = @"
    namespace Sidio.Mediator.SourceGenerator.Tests;

    public class TestRequest1 : IRequest;

    public class TestRequest1Handler : IRequestHandler<TestRequest1>
    {
        public Task<Result> HandleAsync(TestRequest1 request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }";

    private const string SourceCodeTypedRequest = @"
    namespace Sidio.Mediator.SourceGenerator.Tests;

    public class TestRequest1 : IRequest<string>;

    public class TestRequest1Handler : IRequestHandler<TestRequest1, string>
    {
        public Task<Result<string>> HandleAsync(TestRequest1 request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result<string>.Success(""OK""));
        }
    }";

    private const string SourceCodeHttpRequest = @"
    namespace Sidio.Mediator.SourceGenerator.Tests;

    using Sidio.Mediator.Http;

    public class TestRequest1 : IHttpRequest<string>;

    public class TestRequest1Handler : IHttpRequestHandler<TestRequest1, string>
    {
        public Task<HttpResult<string>> HandleAsync(TestRequest1 request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HttpResult<string>.Success(""OK""));
        }
    }";
}