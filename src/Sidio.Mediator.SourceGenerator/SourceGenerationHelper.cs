using System.Text;

namespace Sidio.Mediator.SourceGenerator;

internal static class SourceGenerationHelper
{
    private const string Spacing = "    ";

    public const string MediatorPartialClassSource = @"
namespace Sidio.Mediator
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public partial interface IMediator
    {
    }

    public partial class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }
    }
}";
    
    public static string GenerateClass(RequestToGenerate requestToGenerate)
    {
        var http = requestToGenerate.IsHttpRequest ? "Http" : string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine("namespace Sidio.Mediator");
        sb.AppendLine("{");
        sb.AppendLine($"{Spacing}using System.Threading;");
        sb.AppendLine($"{Spacing}using System.Threading.Tasks;");
        sb.AppendLine($"{Spacing}using Microsoft.Extensions.DependencyInjection;\n");

        if (requestToGenerate.IsHttpRequest)
        {
            sb.AppendLine($"{Spacing}using Sidio.Mediator.Http;");
        }

        if (!string.IsNullOrWhiteSpace(requestToGenerate.NamespaceName))
        {
            sb.AppendLine($"{Spacing}using {requestToGenerate.NamespaceName};");
        }

        // interface
        sb.AppendLine();
        sb.AppendLine($"{Spacing}public partial interface IMediator");
        sb.AppendLine($"{Spacing}{{");
        if (requestToGenerate.ReturnType is not null)
        {
            sb.AppendLine($"{Spacing}{Spacing}Task<{http}Result<{requestToGenerate.ReturnType}>> {requestToGenerate.ClassName}Async({requestToGenerate.ClassName} request, CancellationToken cancellationToken = default);");
        }
        else
        {
            sb.AppendLine($"{Spacing}{Spacing}Task<Result> {requestToGenerate.ClassName}Async({requestToGenerate.ClassName} request, CancellationToken cancellationToken = default);");
        }

        sb.AppendLine($"{Spacing}}}\n"); // interface IMediator

        // class
        sb.AppendLine($"{Spacing}public partial class Mediator");
        sb.AppendLine($"{Spacing}{{");

        if (requestToGenerate.ReturnType is not null)
        {
            sb.AppendLine($"{Spacing}{Spacing}public Task<{http}Result<{requestToGenerate.ReturnType}>> {requestToGenerate.ClassName}Async({requestToGenerate.ClassName} request, CancellationToken cancellationToken = default)");
        }
        else
        {
            sb.AppendLine($"{Spacing}{Spacing}public Task<Result> {requestToGenerate.ClassName}Async({requestToGenerate.ClassName} request, CancellationToken cancellationToken = default)");
        }

        sb.AppendLine($"{Spacing}{Spacing}{{");

        sb.AppendLine($"{Spacing}{Spacing}{Spacing}var requestHandler = _serviceProvider.GetRequiredService<{requestToGenerate.RequestHandlerInterface}>();");
        sb.AppendLine($"{Spacing}{Spacing}{Spacing}return requestHandler.HandleAsync(request, cancellationToken);");

        sb.AppendLine($"{Spacing}{Spacing}}}"); // function
        sb.AppendLine($"{Spacing}}}"); // class Mediator
        sb.AppendLine("}"); // namespace

        return sb.ToString();
    }
}