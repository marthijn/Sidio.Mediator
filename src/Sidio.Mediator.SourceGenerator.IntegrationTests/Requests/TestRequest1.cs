namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class TestRequest1 : IRequest
{
    public TestRequest1(string name)
    {
        Name = name;
    }

    public string Name { get; }
}