using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class VariableRules
    {
        public static void Apply(VariableDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing variable {node.GetText()} in file {filePath}");
        }
    }
}
