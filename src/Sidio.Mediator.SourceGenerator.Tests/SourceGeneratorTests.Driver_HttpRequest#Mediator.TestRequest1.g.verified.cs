//HintName: Mediator.TestRequest1.g.cs
namespace Sidio.Mediator
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    using Sidio.Mediator.Http;
    using Sidio.Mediator.SourceGenerator.Tests;

    public partial interface IMediator
    {
        Task<HttpResult<string>> TestRequest1Async(TestRequest1 request, CancellationToken cancellationToken = default);
    }

    public partial class Mediator
    {
        public Task<HttpResult<string>> TestRequest1Async(TestRequest1 request, CancellationToken cancellationToken = default)
        {
            var requestHandler = _serviceProvider.GetRequiredService<IHttpRequestHandler<TestRequest1, string>>();
            return requestHandler.HandleAsync(request, cancellationToken);
        }
    }
}
