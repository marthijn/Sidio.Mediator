using System.Threading;
using System.Threading.Tasks;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class RequestWithValidationHandler : IRequestHandler<RequestWithValidation>
{
    public Task<Result> HandleAsync(RequestWithValidation request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result.Success());
    }
}