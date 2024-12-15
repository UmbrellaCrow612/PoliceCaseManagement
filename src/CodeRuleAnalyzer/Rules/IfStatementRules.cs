using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class IfStatementRules
    {
        public static void Apply(IfStatementSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing if statement in file {filePath}");
        }
    }
}
