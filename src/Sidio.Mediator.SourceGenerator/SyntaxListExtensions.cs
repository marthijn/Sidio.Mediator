using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sidio.Mediator.SourceGenerator;

internal static class SyntaxListExtensions
{
    public static IEnumerable<string> ToStringEnumerable(this SyntaxList<UsingDirectiveSyntax> usings)
    {
        return usings
            .Where(x => !string.IsNullOrEmpty(x.Name?.ToString()))
            .Select(x => x.Name!.ToString());
    }
}