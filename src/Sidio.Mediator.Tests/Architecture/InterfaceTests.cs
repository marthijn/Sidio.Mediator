using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Sidio.Mediator.Tests.Architecture;

public sealed class InterfaceTests
{
    private static readonly ArchUnitNET.Domain.Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(IRequest).Assembly).Build();

    [Fact]
    public void Interfaces_ShouldStartWithI()
    {
        // Act
        var rule = Interfaces().Should().HaveNameStartingWith("I");

        // Assert
        rule.Check(Architecture);
    }
}