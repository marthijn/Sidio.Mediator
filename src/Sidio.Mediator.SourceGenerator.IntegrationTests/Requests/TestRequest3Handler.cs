using System.Threading;
using System.Threading.Tasks;
using Sidio.Mediator.SourceGenerator.IntegrationTests.Requests.Models;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class TestRequest3Handler : IRequestHandler<TestRequest3, RequestResult>
{
    public Task<Result<RequestResult>> HandleAsync(TestRequest3 request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result<RequestResult>.Success(new RequestResult()));
    }
}