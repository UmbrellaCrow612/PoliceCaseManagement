using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class IfStatementRuleManager
    {
        public static void ApplyAll(IfStatementSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing if statement in file {filePath}");
        }
    }
}
