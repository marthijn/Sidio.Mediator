using System.Runtime.CompilerServices;

namespace Sidio.Mediator.SourceGenerator.Tests;

public static class Initialize
{
    [ModuleInitializer]
    public static void Init() =>
        VerifySourceGenerators.Initialize();
}