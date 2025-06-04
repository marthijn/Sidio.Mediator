using System.Threading;
using System.Threading.Tasks;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class TestRequest1Handler : IRequestHandler<TestRequest1>
{
    public Task<Result> HandleAsync(TestRequest1 request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result.Success());
    }
}