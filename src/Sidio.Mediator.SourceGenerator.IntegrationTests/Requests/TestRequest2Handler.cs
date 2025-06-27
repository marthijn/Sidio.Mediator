using System.Threading;
using System.Threading.Tasks;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class TestRequest2Handler : IRequestHandler<TestRequest2, string>
{
    public Task<Result<string>> HandleAsync(TestRequest2 request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result<string>.Success($"Hello {request.Name}"));
    }
}