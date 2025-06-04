namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class RequestWithValidation : IRequest
{
    public RequestWithValidation(string name)
    {
        Name = name;
    }

    public string Name { get; }
}