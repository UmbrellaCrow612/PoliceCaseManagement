using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class VariableRuleManager
    {
        public static void ApplyAll(VariableDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing variable {node.GetText()} in file {filePath}");
        }
    }
}
