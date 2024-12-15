using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeRuleAnalyzer.Rules
{
    internal class ClassRules
    {
        public static void Apply(ClassDeclarationSyntax node, string filePath)
        {
            Console.WriteLine($"Analyzing class '{node.Identifier.Text}' in file {filePath}");
        }
    }
}
