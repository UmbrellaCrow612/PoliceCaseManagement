using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

internal class IdNameRule : CodeRule.Core.CodeRule
{
    public override void Analyze(SyntaxNode rootNode, string filePath)
    {
        var methodDeclarations = rootNode.DescendantNodes().OfType<MethodDeclarationSyntax>();
        foreach (var method in methodDeclarations)
        {
            foreach (var param in method.ParameterList.Parameters)
            {
                if (param.Identifier.ValueText.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    AddViolation(
                        filePath,
                        param.Identifier,
                        "Parameter name should not be 'ID'",
                        "Warning"
                    );
                }
            }
        }
    }
}