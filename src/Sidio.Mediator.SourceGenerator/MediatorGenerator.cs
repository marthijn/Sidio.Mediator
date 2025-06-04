using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Sidio.Mediator.SourceGenerator;

[Generator]
public sealed class MediatorGenerator : IIncrementalGenerator
{
    private const string FileNamePrefix = "Mediator";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            $"{FileNamePrefix}.g.cs",
            SourceText.From(SourceGenerationHelper.MediatorPartialClassSource, Encoding.UTF8)));

        IncrementalValuesProvider<RequestToGenerate?> requestsToGenerate = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
            transform: static (ctx, _) =>
            {
                return GetSemanticTargetForGeneration(ctx);
            }).Where(static m => m is not null);

        context.RegisterSourceOutput(
            requestsToGenerate,
            static (ctx, requestToGenerate) => Execute(requestToGenerate, ctx));
    }

    private static void Execute(RequestToGenerate? requestToGenerate, SourceProductionContext context)
    {
        if (requestToGenerate is { } value)
        {
            var result = SourceGenerationHelper.GenerateClass(value);
            context.AddSource($"{FileNamePrefix}.{value.ClassName}.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static RequestToGenerate? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
        {
            // the node is not a class declaration, ignore it
            return null;
        }

        var baseList = classDeclaration.BaseList;
        if (baseList == null)
        {
            // the class does not implement any interfaces, ignore it
            return null;
        }

        foreach (var baseType in baseList.Types)
        {
            var typeName = baseType.Type.ToString();
            if (IsValidRequestType(typeName))
            {
                var ns = GetNamespace(classDeclaration);

                // we found a matching interface, return the class name
                return RequestToGenerate.Create(classDeclaration.Identifier.Text, typeName, ns);
            }
        }

        return null;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax m && m.BaseList != null &&
           m.BaseList.Types.Any(t => IsValidRequestType(t.Type.ToString()));

    private static bool IsValidRequestType(string typeName)
    {
        if (typeName.IndexOf("RequestHandler", StringComparison.Ordinal) >= 0)
        {
            return false;
        }

        return  typeName.StartsWith("IRequest", StringComparison.Ordinal) ||
               typeName.StartsWith("IHttpRequest<", StringComparison.Ordinal);
    }

    // determine the namespace the class/enum/struct is declared in, if any
    private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
    {
        //// credits: https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/

        // If we don't have a namespace at all we'll return an empty string
        // This accounts for the "default namespace" case
        string nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration
        // (could be a nested type, for example)
        SyntaxNode? potentialNamespaceParent = syntax.Parent;

        // Keep moving "out" of nested classes etc until we get to a namespace
        // or until we run out of parents
        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax
               && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();

            // Keep moving "out" of the namespace declarations until we
            // run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        // return the final namespace
        return nameSpace;
    }
}