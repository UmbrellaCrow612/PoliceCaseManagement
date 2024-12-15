using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal static class MethodRules
    {
        public static void Apply(MethodDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing method '{node.Identifier.Text}' in file {filePath}");
            // run all method rules to this node
        }
    }
}
