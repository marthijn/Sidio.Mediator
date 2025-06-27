namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class TestRequest2 : IRequest<string>
{
    public TestRequest2(string name)
    {
        Name = name;
    }

    public string Name { get; }
}