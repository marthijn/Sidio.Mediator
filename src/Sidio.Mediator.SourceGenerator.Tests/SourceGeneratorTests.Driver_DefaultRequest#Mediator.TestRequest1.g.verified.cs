//HintName: Mediator.TestRequest1.g.cs
namespace Sidio.Mediator
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Sidio.Mediator.SourceGenerator.Tests;

    public partial interface IMediator
    {
        Task<Result> TestRequest1Async(TestRequest1 request, CancellationToken cancellationToken = default);
    }

    public partial class Mediator
    {
        public Task<Result> TestRequest1Async(TestRequest1 request, CancellationToken cancellationToken = default)
        {
            var requestHandler = _serviceProvider.GetRequiredService<IRequestHandler<TestRequest1>>();
            return requestHandler.HandleAsync(request, cancellationToken);
        }
    }
}
