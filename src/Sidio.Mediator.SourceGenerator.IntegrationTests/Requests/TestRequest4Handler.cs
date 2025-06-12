using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sidio.Mediator.Http;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class TestRequest4Handler : IHttpRequestHandler<TestRequest4, IReadOnlyList<string>>
{
    public Task<HttpResult<IReadOnlyList<string>>> HandleAsync(TestRequest4 request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HttpResult<IReadOnlyList<string>>.Success(new List<string> {"Test1", "Test2"}));
    }
}