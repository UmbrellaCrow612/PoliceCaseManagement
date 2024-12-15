using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class FieldRules
    {
        public static void Apply(FieldDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing field in file {filePath}");
        }
    }
}
